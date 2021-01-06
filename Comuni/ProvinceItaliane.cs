using Comuni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comuni
{
    internal class Province
    {
        public BidirectionalMap<string, string> Mappatura; 

        internal static Provincia[] Elenco = new []
        {
            new Provincia(19,"Sicilia",84,"AG","Agrigento",9.79823),
            new Provincia(1,"Piemonte",6,"AL","Alessandria",9.80496),
            new Provincia(11,"Marche",42,"AN","Ancona",9.80402),
            new Provincia(2,"Valle d'Aosta",7,"AO","Aosta",9.80375),
            new Provincia(0,"Abruzzo", 0,"AQ","L'Aquila",9.80053), // non ho gli altri dati, solo g !!!! trovato anche 9.80129 g !!!!)
            new Provincia(13,"Abruzzo",66,"AQ","L'Aquila",9.80129),
            new Provincia(9,"Toscana",51,"AR","Arezzo",9.80389),
            new Provincia(11,"Marche",44,"AP","Ascoli Piceno",9.80317),
            new Provincia(1,"Piemonte",5,"AT","Asti",9.80471),
            new Provincia(15,"Campania",64,"AV","Avellino",9.80175),
            new Provincia(16,"Puglia",72,"BA","Bari",9.80324),
            new Provincia(5,"Veneto",25,"BL","Belluno",9.80562),
            new Provincia(15,"Campania",62,"BN","Benevento",9.80247),
            new Provincia(3,"Lombardia",16,"BG","Bergamo",9.80471),
            new Provincia(1,"Piemonte",96,"BI","Biella",9.80437),
            new Provincia(8,"Emilia-Romagna",37,"BO","Bologna",9.80419),
            new Provincia(4,"Trentino-Alto Adige",21,"BZ","Bolzano",9.80548),
            new Provincia(3,"Lombardia",17,"BS","Brescia",9.80456),
            new Provincia(16,"Puglia",74,"BR","Brindisi",9.8027),
            new Provincia(20,"Sardegna",92,"CA","Cagliari",9.80096),
            new Provincia(19,"Sicilia",85,"CL","Caltanissetta",9.79676),
            new Provincia(14,"Molise",70,"CB","Campobasso",9.80092),
            new Provincia(15,"Campania",61,"CE","Caserta",9.80265),
            new Provincia(19,"Sicilia",87,"CT","Catania",9.8004),
            new Provincia(18,"Calabria",79,"CZ","Catanzaro",9.80002),
            new Provincia(13,"Abruzzo",69,"CH","Chieti",9.80237),
            new Provincia(3,"Lombardia",13,"CO","Como",9.80516),
            new Provincia(18,"Calabria",78,"CS","Cosenza",9.8012),
            new Provincia(3,"Lombardia",19,"CR","Cremona",9.80511),
            new Provincia(18,"Calabria",101,"KR","Crotone",9.80084),
            new Provincia(1,"Piemonte",4,"CN","Cuneo",9.80264),
            new Provincia(19,"Sicilia",86,"EN","Enna",9.79571),
            new Provincia(8,"Emilia-Romagna",38,"FE","Ferrara",9.80447),
            new Provincia(9,"Toscana",48,"FI","Firenze",9.80483),
            new Provincia(16,"Puglia",71,"FG","Foggia",9.80267),
            new Provincia(8,"Emilia-Romagna",40,"FC","Forlì-Cesena",9.80435),
            new Provincia(12,"Lazio",60,"FR","Frosinone",9.80246),
            new Provincia(7,"Liguria",10,"GE","Genova",9.80559),
            new Provincia(6,"Friuli-Venezia Giulia",31,"GO","Gorizia",9.80636),
            new Provincia(9,"Toscana",53,"GR","Grosseto",9.80425),
            new Provincia(7,"Liguria",8,"IM","Imperia",9.80508),
            new Provincia(14,"Molise",94,"IS","Isernia",9.80161),
            new Provincia(7,"Liguria",11,"SP","La Spezia",9.80552),
            new Provincia(12,"Lazio",59,"LT","Latina",9.8033),
            new Provincia(16,"Puglia",75,"LE","Lecce",9.80247),
            new Provincia(3,"Lombardia",97,"LC","Lecco",9.80519),
            new Provincia(9,"Toscana",49,"LI","Livorno",9.80516),
            new Provincia(3,"Lombardia",98,"LO","Lodi",9.80491),
            new Provincia(9,"Toscana",46,"LU","Lucca",9.80516),
            new Provincia(0,"Lombardia", 0,"MB","Monza-Brianza", 9.80505), // non ho gli altri dati, solo g
            new Provincia(11,"Marche",43,"MC","Macerata",9.80318),
            new Provincia(3,"Lombardia",20,"MN","Mantova",9.8052),
            new Provincia(9,"Toscana",45,"MS","Massa-Carrara",9.80508),
            new Provincia(17,"Basilicata",77,"MT","Matera",9.80072),
            new Provincia(19,"Sicilia",83,"ME","Messina",9.80082),
            new Provincia(3,"Lombardia",15,"MI","Milano",9.80505),
            new Provincia(8,"Emilia-Romagna",36,"MO","Modena",9.80416),
            new Provincia(15,"Campania",63,"NA","Napoli",9.80296),
            new Provincia(1,"Piemonte",3,"NO","Novara",9.80471),
            new Provincia(20,"Sardegna",91,"NU","Nuoro",9.80027),
            new Provincia(20,"Sardegna",95,"OR","Oristano",9.80172),
            new Provincia(5,"Veneto",28,"PD","Padova",9.80652),
            new Provincia(19,"Sicilia",82,"PA","Palermo",9.80054),
            new Provincia(8,"Emilia-Romagna",34,"PR","Parma",9.80427),
            new Provincia(3,"Lombardia",18,"PV","Pavia",9.80481),
            new Provincia(10,"Umbria",54,"PG","Perugia",9.80314),
            new Provincia(11,"Marche",41,"PS","Pesaro e Urbino",9.80439),
            new Provincia(13,"Abruzzo",68,"PE","Pescara",9.80326),
            new Provincia(8,"Emilia-Romagna",33,"PC","Piacenza",9.80459),
            new Provincia(9,"Toscana",50,"PI","Pisa", 9.80513),
            new Provincia(9,"Toscana",47,"PT","Pistoia", 9.805),
            new Provincia(6,"Friuli-Venezia Giulia",93,"PN","Pordenone",9.80629),
            new Provincia(17,"Basilicata",76,"PZ","Potenza",9.7997),
            new Provincia(9,"Toscana",100,"PO","Prato",9.80484),
            new Provincia(19,"Sicilia",88,"RG","Ragusa",9.79769),
            new Provincia(8,"Emilia-Romagna",39,"RA","Ravenna",9.8044),
            new Provincia(18,"Calabria",80,"RC","Reggio di Calabria",9.80063),
            new Provincia(8,"Emilia-Romagna",35,"RE","Reggio nell'Emilia",9.80414),
            new Provincia(12,"Lazio",57,"RI","Rieti",9.80264),
            new Provincia(8,"Emilia-Romagna",99,"RN","Rimini",9.80439),
            new Provincia(12,"Lazio",58,"RM","Roma",9.80352),
            new Provincia(5,"Veneto",29,"RO","Rovigo",9.80605),
            new Provincia(15,"Campania",65,"SA","Salerno",9.80269),
            new Provincia(20,"Sardegna",90,"SS","Sassari",9.80184),
            new Provincia(7,"Liguria",9,"SV","Savona",9.80559),
            new Provincia(9,"Toscana",52,"SI","Siena",9.8038),
            new Provincia(19,"Sicilia",89,"SR","Siracusa",9.80034),
            new Provincia(3,"Lombardia",14,"SO","Sondrio",9.80534),
            new Provincia(16,"Puglia",73,"TA","Taranto",9.80231),
            new Provincia(13,"Abruzzo",67,"TE","Teramo",9.80269),
            new Provincia(10,"Umbria",55,"TR","Terni",9.80359),
            new Provincia(1,"Piemonte",1,"TO","Torino",9.80577),
            new Provincia(19,"Sicilia",81,"TP","Trapani",9.80052),
            new Provincia(4,"Trentino-Alto Adige",22,"TN","Trento",9.80596),
            new Provincia(5,"Veneto",26,"TV","Treviso",9.80631),
            new Provincia(6,"Friuli-Venezia Giulia",32,"TS","Trieste",9.80653),
            new Provincia(6,"Friuli-Venezia Giulia",30,"UD","Udine",9.80609),
            new Provincia(3,"Lombardia",12,"VA","Varese",9.80451),
            new Provincia(5,"Veneto",27,"VE","Venezia",9.80631),
            new Provincia(1,"Piemonte",103,"VB","Verbano-Cusio-Ossola",9.80544),
            new Provincia(1,"Piemonte",2,"VC","Vercelli",9.80465),
            new Provincia(5,"Veneto",23,"VR","Verona",9.80644),
            new Provincia(18,"Calabria",102,"VV","Vibo Valentia",9.79916),
            new Provincia(5,"Veneto",24,"VI","Vicenza",9.80643),
            new Provincia(12,"Lazio",56,"VT","Viterbo",9.80294), 
        };

        public Province()
        {
            Mappatura = new BidirectionalMap<string, string>();
            Mappatura.Add("AG", "AGRIGENTO");
            Mappatura.Add("AL", "ALESSANDRIA");
            Mappatura.Add("AN", "ANCONA");
            Mappatura.Add("AO", "AOSTA");
            Mappatura.Add("AQ", "L'AQUILA");
            Mappatura.Add("AR", "AREZZO");
            Mappatura.Add("AP", "ASCOLI PICENO");
            Mappatura.Add("AT", "ASTI");
            Mappatura.Add("AV", "AVELLINO");
            Mappatura.Add("BA", "BARI");
            Mappatura.Add("BL", "BELLUNO");
            Mappatura.Add("BN", "BENEVENTO");
            Mappatura.Add("BG", "BERGAMO");
            Mappatura.Add("BI", "BIELLA");
            Mappatura.Add("BO", "BOLOGNA");
            Mappatura.Add("BZ", "BOLZANO");
            Mappatura.Add("BS", "BRESCIA");
            Mappatura.Add("BR", "BRINDISI");
            Mappatura.Add("CA", "CAGLIARI");
            Mappatura.Add("CL", "CALTANISSETTA");
            Mappatura.Add("CB", "CAMPOBASSO");
            Mappatura.Add("CE", "CASERTA");
            Mappatura.Add("CT", "CATANIA");
            Mappatura.Add("CZ", "CATANZARO");
            Mappatura.Add("CH", "CHIETI");
            Mappatura.Add("CO", "COMO");
            Mappatura.Add("CS", "COSENZA");
            Mappatura.Add("CR", "CREMONA");
            Mappatura.Add("KR", "CROTONE");
            Mappatura.Add("CN", "CUNEO");
            Mappatura.Add("EN", "ENNA");
            Mappatura.Add("FE", "FERRARA");
            Mappatura.Add("FI", "FIRENZE");
            Mappatura.Add("FG", "FOGGIA");
            Mappatura.Add("FC", "FORLI' CESENA");
            Mappatura.Add("FR", "FROSINONE");
            Mappatura.Add("GE", "GENOVA");
            Mappatura.Add("GO", "GORIZIA");
            Mappatura.Add("GR", "GROSSETO");
            Mappatura.Add("IM", "IMPERIA");
            Mappatura.Add("IS", "ISERNIA");
            Mappatura.Add("SP", "LA SPEZIA");
            Mappatura.Add("LT", "LATINA");
            Mappatura.Add("LE", "LECCE");
            Mappatura.Add("LC", "LECCO");
            Mappatura.Add("LI", "LIVORNO");
            Mappatura.Add("LO", "LODI");
            Mappatura.Add("LU", "LUCCA");
            Mappatura.Add("MB", "MONZA BRIANZA");
            Mappatura.Add("MC", "MACERATA");
            Mappatura.Add("MN", "MANTOVA");
            Mappatura.Add("MS", "MASSA CARRARA");
            Mappatura.Add("MT", "MATERA");
            Mappatura.Add("ME", "MESSINA");
            Mappatura.Add("MI", "MILANO");
            Mappatura.Add("MO", "MODENA");
            Mappatura.Add("NA", "NAPOLI");
            Mappatura.Add("NO", "NOVARA");
            Mappatura.Add("NU", "NUORO");
            Mappatura.Add("OR", "ORISTANO");
            Mappatura.Add("PD", "PADOVA");
            Mappatura.Add("PA", "PALERMO");
            Mappatura.Add("PR", "PARMA");
            Mappatura.Add("PV", "PAVIA");
            Mappatura.Add("PG", "PERUGIA");
            Mappatura.Add("PU", "PESARO URBINO");
            Mappatura.Add("PE", "PESCARA");
            Mappatura.Add("PC", "PIACENZA");
            Mappatura.Add("PI", "PISA");
            Mappatura.Add("PT", "PISTOIA");
            Mappatura.Add("PN", "PORDENONE");
            Mappatura.Add("PZ", "POTENZA");
            Mappatura.Add("PO", "PRATO");
            Mappatura.Add("RG", "RAGUSA");
            Mappatura.Add("RA", "RAVENNA");
            Mappatura.Add("RC", "REGGIO CALABRIA");
            Mappatura.Add("RE", "REGGIO EMILIA");
            Mappatura.Add("RI", "RIETI");
            Mappatura.Add("RN", "RIMINI");
            Mappatura.Add("RM", "ROMA");
            Mappatura.Add("RO", "ROVIGO");
            Mappatura.Add("SA", "SALERNO");
            Mappatura.Add("SS", "SASSARI");
            Mappatura.Add("SV", "SAVONA");
            Mappatura.Add("SI", "SIENA");
            Mappatura.Add("SR", "SIRACUSA");
            Mappatura.Add("SO", "SONDRIO");
            Mappatura.Add("TA", "TARANTO");
            Mappatura.Add("TE", "TERAMO");
            Mappatura.Add("TR", "TERNI");
            Mappatura.Add("TO", "TORINO");
            Mappatura.Add("TP", "TRAPANI");
            Mappatura.Add("TN", "TRENTO");
            Mappatura.Add("TV", "TREVISO");
            Mappatura.Add("TS", "TRIESTE");
            Mappatura.Add("UD", "UDINE");
            Mappatura.Add("VA", "VARESE");
            Mappatura.Add("VE", "VENEZIA");
            Mappatura.Add("VB", "VERBANO-CUSIO-OSSOLA");
            Mappatura.Add("VC", "VERCELLI");
            Mappatura.Add("VR", "VERONA");
            Mappatura.Add("VV", "VIBO VALENTIA");
            Mappatura.Add("VI", "VICENZA");
            Mappatura.Add("VT", "VITERBO"); 
        }
    }
}
