DROP TABLE IF EXISTS delivery_condition;
DROP TABLE IF EXISTS opening_hours;
DROP TABLE IF EXISTS order_item; 
DROP TABLE IF EXISTS menu_item;
DROP TABLE IF EXISTS closing_day;
DROP TABLE IF EXISTS api_key;
DROP TABLE IF EXISTS change_order_status_link; 
DROP TABLE IF EXISTS `order`;
DROP TABLE IF EXISTS restaurant;
DROP TABLE IF EXISTS address;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";
CREATE DATABASE IF NOT EXISTS `SNACKS` 
DEFAULT CHARACTER SET latin1 COLLATE latin1_general_ci;
USE `SNACKS`;

-- create tables
CREATE TABLE address (
  address_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  zipcode INT NOT NULL,
  city VARCHAR(255) NOT NULL,
  street VARCHAR(255) NOT NULL,
  street_number INT NOT NULL,
  gps_longitude DECIMAL(20, 16) NOT NULL,
  gps_latitude DECIMAL(20, 16) NOT NULL,
  free_text VARCHAR(255) NULL
);

CREATE TABLE restaurant (
  restaurant_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  restaurant_name VARCHAR(255) NOT NULL,
  webhook_url VARCHAR(255) NOT NULL,
  image_path VARCHAR(255),
  address_id INT NOT NULL,
  FOREIGN KEY (address_id) REFERENCES address (address_id)
);

CREATE TABLE api_key (
  restaurant_id INT NOT NULL,
  api_key_value VARCHAR(255) NOT NULL,
  created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (restaurant_id, api_key_value)
);


CREATE TABLE closing_day (
  week_day VARCHAR(255) NOT NULL,
  restaurant_id INT NOT NULL,
  PRIMARY KEY (week_day, restaurant_id),
  FOREIGN KEY (restaurant_id) REFERENCES restaurant (restaurant_id)
);

CREATE TABLE opening_hours(
  opening_hours_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  week_day VARCHAR(255) NOT NULL,
  restaurant_id INT NOT NULL,
  start_time TIME NOT NULL,
  end_time TIME NOT NULL,
  FOREIGN KEY (restaurant_id) REFERENCES restaurant (restaurant_id)
);

DELIMITER //

CREATE TRIGGER prevent_insert_opening_hours_conflict_with_closing_day
BEFORE INSERT ON opening_hours
FOR EACH ROW
BEGIN
  DECLARE conflict_count INT;
  
  SELECT COUNT(*) INTO conflict_count
  FROM closing_day
  WHERE week_day = NEW.week_day
  AND restaurant_id = NEW.restaurant_id;
  
  IF conflict_count > 0 THEN
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Cannot insert opening hours due to conflicting closing day';
  END IF;
END//

DELIMITER ;

DELIMITER //

CREATE TRIGGER prevent_insert_closing_day_conflict_with_opening_hours
BEFORE INSERT ON closing_day
FOR EACH ROW
BEGIN
  DECLARE conflict_count INT;
  
  SELECT COUNT(*) INTO conflict_count
  FROM opening_hours
  WHERE week_day = NEW.week_day
  AND restaurant_id = NEW.restaurant_id;
  
  IF conflict_count > 0 THEN
    SIGNAL SQLSTATE '45000'
    SET MESSAGE_TEXT = 'Cannot insert closing day due to conflicting opening hours';
  END IF;
END//

DELIMITER ;



CREATE TABLE menu_item (
  menu_item_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  restaurant_id INT NOT NULL,
  menu_item_name VARCHAR(255) NOT NULL,
  menu_item_description VARCHAR(255),
  price DECIMAL(10,2) NOT NULL, 
  category_name VARCHAR(255) NOT NULL,
  FOREIGN KEY (restaurant_id) REFERENCES restaurant (restaurant_id),
  CONSTRAINT check_price CHECK (price > 0)
);
  
CREATE TABLE delivery_condition (
  delivery_condition_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  restaurant_id INT NOT NULL,
  distance_lower_threshold INT NOT NULL,
  distance_upper_threshold INT NOT NULL,
  order_value_lower_threshold DECIMAL(10,2) NOT NULL,
  order_value_upper_threshold DECIMAL(10,2) ,
  delivery_costs DECIMAL(10,2) NOT NULL,
  min_order_value DECIMAL(10,2) NOT NULL,
  FOREIGN KEY (restaurant_id) REFERENCES restaurant (restaurant_id), 
    CONSTRAINT check_delivery_costs CHECK (delivery_costs >= 0),
    CONSTRAINT check_min_order_value CHECK (min_order_value >= 0),
    CONSTRAINT check_order_value_lower_threshold CHECK (order_value_lower_threshold >= 0),
    CONSTRAINT check_order_value_upper_threshold CHECK (order_value_upper_threshold >= 0),
    CONSTRAINT check_distance_lower_threshold CHECK (distance_lower_threshold >= 0),
    CONSTRAINT check_distance_upper_threshold CHECK (distance_upper_threshold >= 0),
    CONSTRAINT check_distance_range_correct CHECK (distance_upper_threshold > distance_lower_threshold),
    CONSTRAINT check_order_value_range_correct CHECK (order_value_upper_threshold > order_value_lower_threshold OR order_value_upper_threshold IS NULL),
    CONSTRAINT check_min_value CHECK (order_value_lower_threshold >= min_order_value)
);

CREATE TABLE `order` (
  order_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  restaurant_id INT NOT NULL,
  address_id INT NOT NULL,
  customer_name VARCHAR(200) NOT NULL,
  phone_number VARCHAR(200) NOT NULL,
  ordered_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  order_costs DECIMAL(10,2) NOT NULL,
  status VARCHAR(255) NOT NULL,
  FOREIGN KEY (restaurant_id) REFERENCES restaurant (restaurant_id),
  FOREIGN KEY (address_id) REFERENCES address (address_id)
);

CREATE TABLE order_item(
  order_item_id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  order_id INT NOT NULL,
  menu_item_id INT,
  amount INT NOT NULL,
  FOREIGN KEY (order_id) REFERENCES `order` (order_id),
  FOREIGN KEY (menu_item_id) REFERENCES menu_item (menu_item_id) ON DELETE SET NULL,
  CONSTRAINT check_amount CHECK (amount > 0)
);

CREATE TABLE change_order_status_link(
  link VARCHAR(555) NOT NULL PRIMARY KEY,
  order_id INT NOT NULL,
  new_status VARCHAR(255) NOT NULL,
  FOREIGN KEY (order_id) REFERENCES `order` (order_id)
);

-- adding test data

-- address
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude)
VALUES (1010, 'Wien', 'Mariahilfer Straße', 1, '16.367128', '48.194482');
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude)
VALUES (4232, 'Hagenberg', 'Softwarepark', 11, ' 14.514449032326327', '48.368368883920425');
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude)
VALUES (4020, 'Linz', 'Hauptplatz', 1, '14.288194', '48.306940');

-- restaurant
INSERT INTO restaurant (restaurant_name, image_path, webhook_url, address_id)
VALUES ('Schnitzelwirt', 'Schnitzelwirt123.png', 'https://webhook.site/4f131ab5-f565-4d80-a5b2-be471ef81c1e', 1);

INSERT INTO restaurant (restaurant_name, image_path, webhook_url, address_id)
VALUES ('Nimmersatt', 'Nimmersatt123.png', 'https://webhook.site/4f131ab5-f565-4d80-a5b2-be471ef81c1e', 2);

INSERT INTO restaurant (restaurant_name, image_path, webhook_url, address_id)
VALUES ('Premium Food', 'PremiumFood123.png', 'https://webhook.site/4f131ab5-f565-4d80-a5b2-be471ef81c1e', 3);


-- opening_hours
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Wednesday', 1, '11:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Thursday', 1, '11:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Friday', 1, '11:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Saturday', 1, '11:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Sunday', 1, '11:00:00', '22:00:00');

INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Sunday', 2, '11:00:00', '14:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Saturday', 2, '11:00:00', '14:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Tuesday', 2, '11:00:00', '15:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Wednesday', 2, '11:00:00', '15:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Thursday', 2, '11:00:00', '15:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Friday', 2, '11:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Tuesday', 2, '17:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Wednesday', 2, '17:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Thursday', 2, '17:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Friday', 2, '17:00:00', '22:00:00');

INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Friday', 3, '10:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Saturday', 3, '10:00:00', '22:00:00');
INSERT INTO opening_hours (week_day, restaurant_id, start_time, end_time)
VALUES ('Sunday', 3, '10:00:00', '22:00:00');


-- closing_day
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Monday', 1);
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Tuesday', 1);
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Monday', 2);
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Monday', 3);
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Tuesday', 3);
INSERT INTO closing_day (week_day, restaurant_id)
VALUES ('Thursday', 3);



-- menu_item
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Wiener Schnitzel', 'Wiener Schnitzel mit Pommes, Salat und Sauce nach Wahl', 9.90, 'Hauptspeisen');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Cordon Bleu', 'Cordon Bleu mit Pommes, Salat und Sauce nach Wahl', 10.90, 'Hauptspeisen');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Schnitzel Burger', 'Schnitzel Burger mit Pommes, Salat und Sauce nach Wahl', 8.90, 'Hauptspeisen');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Schweinsbraten', 'Schweinsbraten aus Österreich mit Knödel und Sauce nach Wahl', 9.90, 'Hauptspeisen');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Gulasch', 'Gulasch mit Gebäck', 9.90, 'Getränke');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Cola/Fanta/Sprite', '0,5l', 2.50, 'Getränke');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'Bier', '0,5l', 4.50, 'Getränke');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (1, 'kleines Bier', '0,33l', 2.50, 'Getränke');

INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (2, 'Margherita', 'Tomatensauce, Käse', 9.59, 'Pizza');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (2, 'Al Tonno', 'Tomaten, Käse, Thunfisch, Zwiebel, Oliven', 11.00, 'Pizza');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (2, 'Spinaci', 'Tomaten, Käse, Spinat, Feta', 10.00, 'Pizza');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (2, 'Lasagne al Forno', 'mit Rinderfaschiertem', 12.00, 'Pasta');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (2, 'Spaghetti Frutti di Mare', 'mit Meeresfrüchten in Weißweinsauce', 11.00, 'Pasta');

INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (3, 'Heiße Liebe XL', 'Vanilleeis mit Himbeeren aus Spanien', 8.88, 'Nachtisch');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (3, 'Luxus Salat', 'mit Garnelen, Eisbergsalat, Tomaten, Dressing und Gurken', 9.00, 'Salat');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (3, 'Premium Frühlingsrolle', 'mit Faschiertem, Karotten und Kraut', 5.00, 'Vorspeisen');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (3, 'Japanische Sushi', 'mit Lachs, Wasabi und Sojasauce', 22.00, 'Sushi');
INSERT INTO menu_item (restaurant_id, menu_item_name, menu_item_description, price, category_name)
VALUES (3, 'Rotwein', '1/4L', 10.00, 'Getränke');

-- delivery_condition

INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (2, 0, 10, 20, NULL, 0, 20);

INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (2, 11, 50, 20, 29.99, 5, 20);

INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (2, 11, 50, 30, NULL, 0, 20);


INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (1, 0, 30, 30, NULL, 0, 30);

INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (1, 0, 30, 0, 29.99, 8, 0);


INSERT INTO delivery_condition (restaurant_id, 
distance_lower_threshold, distance_upper_threshold, 
order_value_lower_threshold, order_value_upper_threshold, 
delivery_costs, min_order_value)
VALUES (3, 0, 50, 50, NULL, 10, 50);

-- order

-- order 1 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4540, 'Bad Hall', 'Musterstraße', 1, '16.1234', '48.1234', 'Hinterhof');  -- address_id = 4

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 4, 'Hannes Katzenschläger', '+43 6765438456', 'Pending', 12.90);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (1, 1, 2);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (1, 2, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (1, 3, 1);

-- order 2 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Steyr', 'Musterstraße', 10, '16.1234', '48.1234', 'Zimmer 5');  -- address_id = 5

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 5, 'Manfred Katzenschläger', '+43 6765438456', 'Pending', 9.90);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (2, 1, 1);

-- order 3 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4232, 'Hagenberg', 'Software Park', 23, '16.1234', '48.1234', 'Zimmer 5211');  -- address_id = 6

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 6, 'Max Haleluja', '+43 4545132132', 'Pending', 19.80);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (3, 4, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (3, 5, 1);

-- order 4 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4232, 'Hagenberg', 'Software Park', 23, '16.1234', '48.1234', 'Zimmer 1111');  -- address_id = 7

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 7, 'Max Peter Trump', '+43 4545132132', 'BeingPrepared', 20.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (4, 6, 8);

-- order 5 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 7, 'Joe Dostler', '+43 45643131', 'InDelivery', 10.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (5, 11, 1);

-- order 6 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 3, 'Martin Beger', '+43 45545421', 'Delivered', 10.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (6, 11, 1);

-- order 7 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 3, 'Martin Schwarzenegger', '0681 54651212', 'Pending', 22.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (7, 17, 1);

-- order 8 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 1, 'Kerstin Biber', '0681 95543356', 'BeingPrepared', 10.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (8, 18, 1);

-- order 9 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Linz', 'Studentenheimweg', 23, '16.1234', '48.1234', 'Zimmer 8888');  -- address_id = 8

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 8, 'My Fun Person', '0681 46542121', 'InDelivery', 33.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (9, 13, 3);

-- order 10 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Linz', 'Musterweg', 253, '16.1234', '48.1234', 'Garage');  -- address_id = 9

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 9, 'Your Friend Person', '0681 2344543', 'BeingPrepared', 23.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (10, 12, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (10, 13, 1);


-- order 11 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 3, 'Karli Nehummer', '0681 48177', 'Pending', 49.50);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (11, 5, 5);

-- order 12 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 2, 'Christoph Leitl', '0681 2343454', 'BeingPrepared', 109.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (12, 2, 10);

-- order 13 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 1, 'Huns Jakob', '0681 2343454', 'Delivered', 89.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (13, 3, 10);

-- order 14 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Linz', 'Halelujaweg', 111, '16.1234', '48.1234', 'Dachboden');  -- address_id = 10

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 10, 'Hans Zou', '0681 3243434', 'BeingPrepared', 20.59);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (14, 9, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (14, 10, 1);

-- order 15 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 5, 'Alex Holzer', '0681 1111111', 'Pending', 60.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (15, 12, 5);

-- order 16 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 6, 'Mergim Häusler', '0681 8851256', 'InDelivery', 55.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (16, 13, 5);

-- order 17 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 7, 'Karl Marx', '0681 781238621', 'InDelivery', 18.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (17, 15, 2);

-- order 18 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 8, 'Friedrich Engels', '0681 913135456', 'Delivered', 60.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (18, 18, 6);

-- order 19 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 9, 'Johann Gondosch', '0681 84613123', 'Pending', 44.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (19, 17, 2);

-- order 20 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 10, 'David Fuchs', '0681 89911215', 'BeingPrepared', 29.70);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (20, 1, 3);

-- order 21 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4540, 'Bad Hall', 'Moritz-Mitter-Gasse', 888, '16.1234', '48.1234', 'Top 5');  -- address_id = 11

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 11, 'Hannes Katzenschläger', '+43 6765438456', 'Pending', 35.60);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (21, 3, 4);

-- order 22 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Steyr', 'Michaeler Platz', 4, '16.1234', '48.1234', 'Unter dem Baum');  -- address_id = 12

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 12, 'Lisa Jetzinger', '+43 243434', 'Delivered', 30.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (22, 11, 3);

-- order 23 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 12, 'Marie Denk', '0699 1025692', 'BeingPrepared', 40.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (23, 16, 8);

-- order 24 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 12, 'Hannes Wolf', '+43 6765438456', 'Pending', 12.40);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (24, 5, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (24, 6, 1);

-- order 25 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 12, 'Paul Wolf', '+43 8151213', 'InDelivery', 99.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (25, 11, 9);

-- order 26 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4400, 'Steyr', 'Finkenweg', 3, '16.1234', '48.1234', 'im Garten abstellen');  -- address_id = 13

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (2, 13, 'Oma Wolf', '+43 3454354565', 'Pending', 30.59);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (26, 9, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (26, 10, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (26, 11, 1);

-- order 27 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 13, 'Opa Wolf', '0681 2343454', 'BeingPrepared', 110.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (27, 17, 5);

-- order 28 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4522, 'Sierning', 'Holznerweg', 22, '16.1234', '48.1234', 'vor der Tür');  -- address_id = 14

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (1, 14, 'Hannes Großalber', '+43 6765438456', 'Pending', 10.90);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (28, 2, 1);

-- order 29 start
INSERT INTO address (zipcode, city, street, street_number, gps_longitude, gps_latitude, free_text)
VALUES (4522, 'Sierning', 'Steyrweg', 12, '16.1234', '48.1234', 'Top 2');  -- address_id = 14

INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 15, 'Maria Großalber', '+43 8561313212', 'InDelivery', 45.00);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (29, 15, 5);

-- order 30 start
INSERT INTO `order` (restaurant_id, address_id, customer_name, phone_number, status, order_costs)
VALUES (3, 14, 'Hanna Winninger', '+0588 6765438456', 'BeingPrepared', 54.88);

INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (30, 14, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (30, 15, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (30, 16, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (30, 17, 1);
INSERT INTO order_item (order_id, menu_item_id, amount)
VALUES (30, 18, 1);
    	
-- insert api_keys (for all test restaurants = testApiKey123)
INSERT INTO api_key (restaurant_id, api_key_value)
VALUES (1, 'testApiKey123'); 

INSERT INTO api_key (restaurant_id, api_key_value)
VALUES (2, 'testApiKey123');

INSERT INTO api_key (restaurant_id, api_key_value)
VALUES (3, 'testApiKey123');

COMMIT; 
