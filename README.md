# Utilities

- `GPCUtilities`: utilità comuni delle librerie GPC (netstandard2.0, senza dipendenze da Windows).
- `GPCUtilities.Graphics`: grafici bitmap (`GraphicCreator`) e font installati (`FontUtilities`), stesso namespace `GPC.Utilities.Graphics`. È un assembly a parte perché `System.Drawing.Common` funziona solo su Windows da .NET 6 (revisione di architettura, R10): chi usa queste classi aggiunge il riferimento a questo assembly.
- `GPC.Utilities.Serialization` e `Clone.CloneObjectByMemoryStream` usano BinaryFormatter e sono obsoleti: BinaryFormatter è insicuro con dati di altri ed è stato rimosso da .NET 9.
