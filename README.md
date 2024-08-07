# SNACKS
**Students' Nifty And Convenient Kitchen Service**

## Team
- Christopher Nobis
- Yang Zou

## Entwicklungsprozess
- Sourcecodeverwaltung: **Github**
- Ordnerstruktur: 
    - `/src/SNACKS`: Sourcecode
    - `/img`: Bilder für die Dokumentation
    - `/db`: Skripte für die Datenbank
    - `/README.md`: Dokumentation
- Zusammenarbeit: 
    Zu Beginn werden die Aufgaben für die jeweilige Ausbaustufe festgelegt. Für jede Aufgabe wird ein Issue erstellt. Die Issues werden dann auf die Teammitglieder verteilt. Die Issues werden in einem eigenen Branch bearbeitet. Wenn die Aufgabe fertig ist, kann ein Pull Request erstellt werden. Der Pull Request wird dann von dem anderen Teammitglied reviewed und anschließend gemerged in den develop-Branch. Wenn alle Issues für eine Ausbaustufe fertig sind, besprechen wir die Ergebnisse der Ausbaustufe gemeisam und erstellen die Dokumentation. Anschließend wird der develop-Branch in den master-Branch gemerged.
- Issue-Tracking-System: **Github Issues**
- Build-Pipeline: **Github Actions**

## Dokumentation 

1. Für welches Datenmodell haben Sie sich entschieden? <br>
ER-Diagramm (Modellierung mithilfe von Erwin), etwaige Besonderheiten erklären.

    ![ER-Diagramm](./img/er_diagram.png)

    Für den Datenbankentwurf haben wir uns für ein ER-Diagramm entschieden. Ein ER-Diagramm ist ein grafisches Modell, das die Beziehungen zwischen verschiedenen Datentypen darstellt. Es ist ein geeignetes Modell für das vorliegende Projekt, da es die komplexen Beziehungen zwischen den verschiedenen Entitäten, wie Restaurants, Speisen und Lieferbedingungen, gut abbildet.

    Die Besonderheiten des Diagramms sind folgende:

    - Es gibt eine 1:n-Beziehung zwischen Restaurants und Adressen. Ein Restaurant hat eine Adresse, aber eine Adresse kann zu mehreren Restaurants gehören. Beispielsweise kann ein großes Einkaufszentrum mehrere Restaurants haben.

    - Bei den Lieferbedingungen haben wir uns entschieden, dass für jede Lieferbedingung eine Unter- und Obergrenze für die Entfernung und den Bestellwert festgelegt wird. Für die Erfüllung der Lieferbedinung muss die Entfernung größer gleich als die Untergrenze und kleiner gleich als die Obergrenze sein. Gleiches gilt für den Bestellwert. Wenn es keine explizite Obergrenze für den Bestellwert gibt, dann wird die Obergrenze auf null gesetzt. Für die Entfernung wird angenommen, dass es immer eine Ober- und Untergrenze existiert, denn schließlich kann ein Restaurant nicht unendlich weit liefern. Die Distanz wird immer in ganzen Kilometern angegeben.

2. Dokumentieren Sie auf Request-Ebene den gesamten Workflow anhand eines durchgängigen Beispiels (von der Registrierung eines Restaurants bis zur Abfrage des Bestellstatus).
*HTTP-Requests inkl. HTTP-Verb, URL, Parametern, Body und Headern*

    Für eine bessere Übersichtlichkeit werden für die nachfolgenden HTTP-Requests folgende Variablen verwendet:

    `@url = https://localhost:7000`

    `@content-type = application/json`

    
    **- Registrierung eines Restaurants**

    ```json 
    POST {{url}}/api/restaurant/register
    Accept: {{content-type}}
    Content-Type: {{content-type}}

    {
    "restaurant": {
        "restaurantName": "NewBurgerei",
        "webhookUrl": "https://webhook.site/4f131ab5-f565-4d80-a5b2-be471ef81c1e",
        "image": {
        "fileType": "png",
        "imageData": "iVBORw0KGgoAAAANSUhEUgAAABQAAAARCAYAAADdRIy+AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAEnQAABJ0Ad5mH3gAAABdaVRYdFNuaXBNZXRhZGF0YQAAAAAAeyJjbGlwUG9pbnRzIjpbeyJ4IjowLCJ5IjowfSx7IngiOjIwLCJ5IjowfSx7IngiOjIwLCJ5IjoxN30seyJ4IjowLCJ5IjoxN31dfWwy5SQAAAAkSURBVDhPY/wPBAxUBExQmmpg1EDKwaiBlINRAykHI85ABgYAxpMEHuuzdTcAAAAASUVORK5CYII="
        }
    },
    "address": {
        "zipcode": 4232,
        "city": "Hagenberg",
        "street": "Softwarepark",
        "streetNumber": 30,
        "gpsLongitude": 13.388860,
        "gpsLatitude": 52.516275
    },
    "openingHours": [
        {
        "weekDay": "Monday",
        "startTime": "08:00:00",
        "endTime": "17:00:00"
        },
        {
        "weekDay": "Tuesday",
        "startTime": "09:00:00",
        "endTime": "18:00:00"
        },
        {
        "weekDay": "Wednesday",
        "startTime": "09:00:00",
        "endTime": "18:00:00"
        },
        {
        "weekDay": "Thursday",
        "startTime": "09:00:00",
        "endTime": "18:00:00"
        },
        {
        "weekDay": "Friday",
        "startTime": "09:00:00",
        "endTime": "18:00:00"
        }
    ],
    "closingDays": [
        {
        "weekDay": "Saturday"
        },
        {
        "weekDay": "Sunday"
        }
    ]
    }
    ```

    ![ErgebnisRegistrierung](./img/UseCase1.png)

    **- Festlegung der Lieferbedingungen**

    ```json
    POST {{url}}/api/deliverycondition/4
    Accept: {{content-type}}
    Content-Type: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd

    {
        "distanceLowerThreshold": 0,
        "distanceUpperThreshold": 50,
        "orderValueLowerThreshold": 10, 
        "deliveryCosts": 0,
        "minOrderValue": 10
    }
    ```

    ![ErgebnisLieferbedingungen](./img/UseCase2.png)

    **-Aktualisierung der Lieferbedingungen**
    
    ```json
    PUT {{url}}/api/deliverycondition/4/7
    Content-Type: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd

    {
        "distanceLowerThreshold": 0,
        "distanceUpperThreshold": 50,
        "orderValueLowerThreshold": 10, 
        "deliveryCosts": 2,
        "minOrderValue": 10
    }
    ```
    ![ErgebnisAktualisierungLieferbedingungen](./img/UseCase3.png)

    **- Hochladen der Speisekarte**

    ```json
    POST {{url}}/api/menuitem/restaurant/4
    Accept: {{content-type}}
    Content-Type: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd

    [
        {
            "menuItemName": "Big Mac New",
            "menuItemDescription": "mit Pommes und Ketchup",
            "price": 13.50,
            "categoryName": "Hauptspeise"  
        },
        {
            "menuItemName": "Chicken Nuggets",
            "menuItemDescription": "mit Ketchup",
            "price": 7.50,
            "categoryName": "Beilage"  
        }, 
        {
            "menuItemName": "Coca Cola",
            "menuItemDescription": "0,5l",
            "price": 3.50,
            "categoryName": "Getränk"  
        }
    ]
    ```
    ![ErgebnisAktualisierungSpeisekarte](./img/UseCase4.png)

    **- Aktualisierung der Speisekarte**
    
    ```json
    PUT {{url}}/api/menuitem/restaurant/4
    Accept: {{content-type}}
    Content-Type: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd

    [
        {
            "menuItemId": 19,
            "menuItemName": "Big Mac New",
            "menuItemDescription": "mit Pommes und Ketchup",
            "price": 15.50, 
            "categoryName": "Hauptspeise"
        },
        {
            "menuItemId": 21,
            "menuItemName": "Coca Cola",
            "menuItemDescription": "0,5l",
            "price": 4.50, 
            "categoryName": "Getränke"
        }
    ]
    ```
    ![ErgebnisAktualisierungSpeisekarte](./img/UseCase5.png)

    **- Student sucht nach Restaurants**

    ```json
    GET {{url}}/api/order/findRestaurants?maxDistance=200&latitude=48.456512&longitude=14.89458&shouldBeOpen=true
    Accept: {{content-type}}
    ```	
    ![ErgebnisSucheNachRestaurants](./img/UseCase6.png)

    **- Student fragt Speisekarte eines Restaurants ab**

    ```json
    GET {{url}}/api/menuitem/restaurant/4
    Accept: {{content-type}}
    ```	
    ![ErgebnisSpeisekarte](./img/UseCase7_1.png)
    ![ErgebnisSpeisekarte](./img/UseCase7.png)

    **- Student fragt die Kosten für eine Auswahl von Speisen ab**

    ```json
    POST {{url}}/api/order/calculatePrice
    Accept: {{content-type}}
    Content-Type: {{content-type}}


    {
        "RestaurantId" : 4,
        "DeliveryAddress": {
            "Zipcode": 4540, 
            "City": "Bad Hall",
            "Street": "Musterstraße 1",
            "StreetNumber": 12,
            "GpsLatitude": 52.516,
            "GpsLongitude": 13.38,
            "FreeText": "Hinterhof"
        },
        "OrderedItems" : [
            {
                "MenuItemId": 19,
                "Amount": 2
            },
            {
                "MenuItemId": 20,
                "Amount": 1
            }
        ]
    }

    ```	
    ![ErgebnisKosten](./img/UseCase8.png)


    **- Student bestellt Speisen**

    ```json
    POST {{url}}/api/order/placeOrder
    Accept: {{content-type}}
    Content-Type: {{content-type}}


    {
        "Order": {
            "RestaurantId" : 4,
            "CustomerName" : "Franz Huber",
            "PhoneNumber" : "0664 1234567"
        },
        "DeliveryAddress": {
            "Zipcode": 4540, 
            "City": "Bad Hall",
            "Street": "Musterstraße 1",
            "StreetNumber": 12,
            "GpsLatitude": 52.516,
            "GpsLongitude": 13.38,
            "FreeText": "Hinterhof"
        },
        "OrderedItems" : [
            {
                "MenuItemId": 19,
                "Amount": 2
            },
            {
                "MenuItemId": 20,
                "Amount": 1
            }
        ]
    }

    ```	
    ![ErgebnisBestellung](./img/UseCase9.png)

    **- Restaurant erhält Bestellung**

    ![ErgebnisBestellung](./img/UseCase9_1.png)
    ![ErgebnisBestellung](./img/UseCase9_2.png)

    

    **- Restaurant aktualisiert Bestellstatus**

    ```json
    PUT {{url}}/api/order/status/4/31
    Accept: {{content-type}}
    Content-Type: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd

    {
        "Status": "InDelivery"
    }
    ```	

    ![ErgebnisBestellstatus](./img/UseCase10.png)

    **- Restaurant aktualisiert Bestellstatus via Link**

    Zur Demonstration wird der Link durch GET-Request erhalten. Anschließend wird der Link in einem Browser geöffnet. 

    Update auf Status `Delivered`:

    ```json
    GET {{url}}/api/order/status/link/4/31/Delivered
    Accept: {{content-type}}
    X-API-KEY: nUT5nPKaQEvtlBOgpyFZ08gCJooUURTnFFUsCd
    ```	

    Link: "https://localhost:7000/api/order/status/executelink?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJPcmRlcklkIjoiMzEiLCJSZXN0YXVyYW50SWQiOiI0IiwiTmV3U3RhdHVzIjoiRGVsaXZlcmVkIiwiQXBpS2V5VmFsdWUiOiJuVVQ1blBLYVFFdnRsQk9ncHlGWjA4Z0NKb29VVVJUbkZGVXNDZCIsIm5iZiI6MTcwMzE3MjkwOSwiZXhwIjoxNzAzMTc2NTA5LCJpYXQiOjE3MDMxNzI5MDl9.z7cziiJlbP0jEmwtKJIQyjzFkQzZbOQxt9k1oDJ0jbw"

    Ausführung im Browser: 

    ![ErgebnisBestellstatus](./img/UseCase11.png)

    **- Student fragt Bestellstatus ab**

    ```json
    GET {{url}}/api/order/status/31
    Accept: {{content-type}}
    ```	
    ![ErgebnisBestellstatus](./img/UseCase12.png)

3. Wie stellen Sie sicher, dass manche Requests nur mit einem gültigen API-Key aufgerufen werden können?

    Es wurde eine Middleware-Klasse `ApiKeyMiddleware` implementiert, die vor dem Aufruf der API-Methoden überprüft, ob ein gültiger API-Key mitgeschickt wurde. Dafür wird der Header `X-Api-Key` verwendet. Wenn der API-Key nicht gültig ist, wird ein Fehler zurückgegeben. Die Middleware-Klasse wird in `Program.cs` registriert. Dabei wird festgelegt, dass die Middleware-Klasse bei folgenden HTTP-Methoden ausgeführt wird: 
    - `GET` (nur bei `/api/order/status/link/{restaurantId}/{orderId}/{newStatus}`)
    - `POST` (ausgenommen bei `/api/restaurant/register`, `/api/order/placeOrder` und `api/order/calculatePrice`)
    - `Put` (alle Methoden)
    - `Delete` (alle Methoden)

    Die Bedingung sieht im Code wie folgt aus:
    
    ```csharp
    app.UseWhen(context => ((context.Request.Method != "GET") && 
                        (!context.Request.Path.StartsWithSegments("/api/restaurant/register")) &&
                        (!context.Request.Path.StartsWithSegments("/api/order/placeOrder")) &&
                        (!context.Request.Path.StartsWithSegments("/api/order/calculatePrice"))) || 
                       (context.Request.Path.StartsWithSegments("/api/order/status/link")), builder =>
    {
    builder.UseMiddleware<ApiKeyMiddleware>();
    });
    ```

    Der Api-Key wird in der Datenbank gespeichert. Beim Registrieren eines Restaurants wird ein Api-Key generiert und in der Datenbank gespeichert. Dafür gibt es die Klasse `ApiKeyGenerator` Beim Aufruf der API-Methoden wird der Api-Key mitgeschickt. Der Api-Key wird dann mit dem Api-Key in der Datenbank verglichen. Wenn die Api-Keys übereinstimmen, dann ist der Api-Key gültig.

4. Welche Teile Ihres Systems sind aus Ihrer Sicht am kritischsten? Welche Maßnahmen haben Sie getroffen, um eine korrekte Funktionsweise sicherzustellen?

    Datenbankzugriff: Sicherstelltung der Funktionalität durch zahlreiche Unit-Tests. Ordnungsgemäße Freigabe von Ressourcen durch Verwendung von `using`. Parameterisierte Abfragen zur Vermeidung von SQL-Injection.

    Absicherung der API: Absicherung mittels API-Key. Überprüfung der API-Key mittels Middleware (siehe Frage 3). Es wird auch darauf geachtet, dass jedes Restaurant nur die eigenen Daten (DeliveryConidtion, MenuItem etc.) ändern kann. Es wird also überprüft,ob jetzt beispielsweise das MenuItemObjekt zum Restaurant gehört, zu dem der Api-Key gehört. Wenn nicht, dann wird Bad Request retourniert.

    Sicherstellung der Korrektheit bei der Berechnung des Gesamtpreises: siehe Frage 5.

5. Wie stellen Sie sicher, dass ihre API bei der Berechnung des Gesamtpreises für eine Bestellung inkl. Lieferkosten in allen Fällen ein korrektes Ergebnis liefert?

    Die Berechnung des Gesamtpreises für eine Bestellung inkl. Lieferkosten wird in der Klasse `OrderController` in der Methode `CalculatePrice` durchgeführt. Dabei wird der zuerst der Wert der bestellten Sachen überprüft, also die Summe der Preise der bestellten MenuItems. Dabei wird angenommen, dass man bei einer Bestellung nur Sachen von einem Restaurant bestellen kann. Im anderen Fall wird Bad Request retourniert. 

    Dann wird die Distanz zwischen dem Restaurant und der Lieferadresse berechnet. Anschließend wird nach einer Lieferbedingung (DeliveryCondition) gesucht, wobei die Distanz zwischen der Untergrenze und der Obergrenze der Lieferbedingung liegen muss. Auch der Wert der bestellten Sachen muss zwischen den entsprechen Grenzen liegen. Wenn es keine Lieferbedingung gibt, die die Bedingungen erfüllt, dann wird Bad Request mit entsprechender Fehlermeldung retourniert.

    Zur Sicherstellung, dass die Lieferbedingunen sich nicht überschneiden, wird beim Registrieren einer Lieferbedingung überprüft (Methode IsDeliveryConditionValidAsync), ob es schon eine Lieferbedingung gibt, die die gleiche Untergrenze und Obergrenze für die Distanz und den Bestellwert hat. Wenn ja, dann wird Bad Request mit entsprechender Fehlermeldung retourniert.

6. Welche Maßnahmen haben Sie getroffen, um sicherzustellen, dass alle Bestellungen tatsächlich beim Restaurant ankommen, auch wenn dessen API kurzfristig nicht erreichbar 
ist?

    Es wurde die Klasse `SendOrderToWebHookUrlService` implementiert, die die Bestellung an die WebHook-URL des Restaurants sendet. Dabei werden maximal 30 Versuche unternommen, um die Bestellung zu senden. Wenn die Bestellung nicht gesendet werden kann, dann wird bei erstem Versuch 3 Sekunden gewartet, beim zweiten Versuch 6 Sekunden, beim dritten Versuch 9 Sekunden usw. Die Wartezeit wird immer um 3 Sekunden erhöht. Wenn die Bestellung nach 30 Versuchen nicht gesendet werden kann, dann ist davon auszugehen, dass die WebHook_URL des Restaurants längerfristig nicht erreichbar ist. 

7. Ein Restaurant behauptet, keine Bestellungen übermittelt zu bekommen. Wie analysieren Sie das Problem?

    Die Klasse `SendOrderToWebHookUrlService` loggt die Information, dass nach der maximalen Anzahl der unternommenen Versuche die WebHookUrl des Restaurants immer noch nicht erreichbar ist. Wenn also ein Restaurant behauptet, keine Bestellungen bekommen zu haben, kann man dann im Log nachschauen, ob es solche Einträge gibt. Wenn ja, dann ist davon auszugehen, dass die WebHookUrl des Restaurants nicht erreichbar ist. Das Problem liegt also beim Restaurant. Wenn nein, dann hat es auch keine Bestellungen für das Restaurant gegeben.

8. Wie haben Sie sichergestellt, dass die Lieferbedingungen (Mindestpreis und Versandbedingungen) möglichst einfach erweiterbar sind? Beispielsweise könnten die Versandkosten von der Postleitzahl abhängig werden. *Beschreiben Sie die relevanten Stellen des Designs*
(*Klassen-Diagramm*).

    Bei einer Erweiterung der Bedingung um die Postleitzahl kann bespielsweise eine neue Spalte `ZipCode` zur Tabelle `DeliveryCondition` hinzugefügt werden. Dadurch ist die Lieferbedingung auch abhängig von der Postleitzahl. Die Untergrenze der Distanz der Lieferbedinung kann auf 0 und die Obergrenze auf einen möglichst hohen Wert gesetzt werden, solange die Distanz zwischen Restaurant und der Stadt (Postleitzahl) kleiner als die Obergrenze ist.

    Ein mögliches Diagramm:
    
    ![Klassendiagramm](./img/DeliveryCondition.png)

9. Welche Teile Ihres Systems sind aus Ihrer Sicht am kritischsten? Welche Maßnahmen haben Sie getroffen, um eine korrekte Funktionsweise sicherzustellen?

    Siehe Frage 4.

10. Denken Sie an die Skalierbarkeit Ihres Projekts: Plötzlich verwenden tausende Restaurants und zehntausende Studierende das Produkt. Was macht Ihnen am meisten Kopfzerbrechen?

    Einige Berechnungen wie z. B. die Ermittlung des Bestellwertes werden von Controllern ausgeführt. Durch effiziente Joins könnte dies bereits in der Datenbank durchgeführt werden, um durch eine frühe Kombination der Daten vor Übergabe an das Backend die Netzwerkauslastung zu reduzieren. Dies könnte jedoch in einem Konflikt mit dem DAO-Pattern stehen, welches um eine Trennung von Datenbankzugriffsschicht und Geschäftslogik bemüht ist.

    Das Projekt verwendet Integer als Primärschlüssel für die verschiedenen Entitäten. Obwohl dieser Datentyp in MySql und C# Werte im Milliardenbereich repräsentieren kann, wären GUIDs aufgrund ihrer Eindeutigkeit eine geeignetere Alternative in verteilten, auf Skalierung ausgelegten Systemen.

12. Wenn Sie das Projekt neu anfangen würden – was würden Sie anders machen?

    Zu Beginn des Projekts wäre eine genauere Analyse der Anforderungen vor Erstellung des Datenmodells sinnvoll gewesen, da sich nachträgliche Änderungen am Datenmodell als aufwendig erwiesen haben.

    Die Build-Pipeline war zunächst so konfiguriert, dass diese bei beinahe jedem Push ausgelöst wurde. Dies führte zu vielen abgebrochenen Build-Vorgängen.
