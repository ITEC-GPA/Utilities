# Regole per gli agenti (librerie GPC: Utilities)

- **Versioni**: le assegna l'utente. Non cambiare le versioni dell'assembly senza averglielo chiesto; sui branch di lavoro restano quelle di master.
- **Rilascio**: un solo treno per Utilities, Geometry, Model e Checker, descritto in `../Checker/build/RILASCIO.txt`. `bin/Release` si ricompila e si committa solo nel commit di versione su master; poi Model aggiorna versione e SHA-256 in `build/dependencies.props`.
- **Git**: ogni lavoro parte da master aggiornato su un branch dedicato; merge su master e push solo con l'ok dell'utente.
