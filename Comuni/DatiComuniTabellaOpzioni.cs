using System;
using System.Collections.Generic;
using System.Text;

namespace Comuni
{
    public class DatiComuniTabellaOpzioni
    {
        string id;
        string descrizioneBreve;
        string descrizione;
        string nomeTabella;
        string nomePrimaryKey; 
        public string Id { get => id; set => id = value; }
        public string DescrizioneBreve { get => descrizioneBreve; set => descrizioneBreve = value; }
        public string Descrizione { get => descrizione; set => descrizione = value; }
        public string NomeTabella { get => nomeTabella; set => nomeTabella = value; }
        public string NomePrimaryKey { get => nomePrimaryKey; set => nomePrimaryKey = value; }
    }
}
