# fswt_cocome
Implementierung des [CoCoME Paper](https://cocome.org/downloads/documentation/cocome.pdf)
für das Modul Fortgeschrittene Softwaretechnologien

# Dokumentation
Sämtliche Dokumentation kann in [architecture](https://github.com/t0rb3n/fswt_cocome/tree/master/architecture) gefunden werden.
Dort enthalte ist die Datei `Dokumentation.pdf`, die sämtliche Diagramme des Referenzpapers enthält, die an unsere Version angepasst wurden.
Für eine kurze Übersicht über das Projekt zu bekommen, existiert außerdem [diese Präsentation](https://github.com/t0rb3n/fswt_cocome/blob/master/architecture/presentation/CoCoME.pdf).


# Starten des Projekts
Voraussetzung zum Starten ist das Projekt [georghinkel/FostDevices](https://github.com/georghinkel/FostDevices).

Folgendes muss zum korrekten Ausführen des Tradingsystems gestartet werden:

- [Fost Terminal](https://github.com/georghinkel/FostDevices/tree/master/Terminal)
- [Bank Server](https://github.com/georghinkel/FostDevices/tree/master/BankServer)
- WebServerEnterprise
- WebServerStore
- CashDesk

> Wichtig ist, dass der CashDesk erst gestartet wird, wenn die anderen Anwendungen verfügbar sind.


# Kurze Übersicht über die einzelnen Projekte

## Application
Umfasst den Application Layer. Enthält die meiste Businesslogik.

## CashDesk
Der CashDesk ist für das Abhandeln der einzelnen Verkäufe verantwortlich. Er kommuniziert ausschließlich mit dem WebServerStore und den FostDevices.

## Data
Die Data bzw. Persistenzschickt der Anwendung. Dieses Projekt ist ausschließlich für die Verbindung zur Datenbank und den damit verbundenen Operationen zuständig.

## GrpcModule
Dient als überliegendes Projekt, dass von CashDesk, WebserverStore und WebServerEnterprise benutzt wird. Sinn ist es, die PROTO Dateien zu teilen.

## WebServerEnterprise
Die Schnittstelle für den Store um an Informationen zu gelangen.

### WebServerEnterprise/EnterpriseApp
Hier befindet sich das Angular Frontend für das Enterprise.

## WebServerStore
Dieses Projekt ist die Schnittstelle für den CashDesk. 

## WebServerStore/StoreApp
Das Angular Frontend für einen Store.

