# Regole per gli agenti (librerie GPC: Utilities)

- **Versioni**: l'utente delega agli agenti la scelta in base alle modifiche. Per i fix incrementare l'ultimo componente; per le altre modifiche incrementare il secondo componente da sinistra e azzerare quelli successivi. Esempi: 0.0.27.1 -> 0.0.27.2 (fix), 0.0.27.2 -> 0.1.0.0 (altro). Registrare motivo e versione nel commit; nessuna richiesta di conferma per il numero. Restano i controlli del set coerente e le autorizzazioni Git.
- **Rilascio**: un solo treno per Utilities, Geometry, Model e Checker, descritto in `../Checker/build/RILASCIO.txt`. `bin/Release` si ricompila e si committa solo nel commit di versione su master; poi Model aggiorna versione e SHA-256 in `build/dependencies.props`.
- **Git**: ogni lavoro parte da master aggiornato su un branch dedicato; merge su master e push solo con l'ok dell'utente.
