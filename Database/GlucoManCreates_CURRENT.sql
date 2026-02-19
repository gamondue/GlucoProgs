--
-- File generated with SQLiteStudio v3.4.17 on mar feb 24 00:26:45 2026
--
-- Text encoding used: UTF-8
--
PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Table: Campioni_Interventi
DROP TABLE IF EXISTS Campioni_Interventi;
CREATE TABLE Campioni_Interventi (IdIntervento INTEGER PRIMARY KEY, IdCampione INTEGER);

-- Table: DatiPunti
DROP TABLE IF EXISTS DatiPunti;
CREATE TABLE DatiPunti (
    IdPunto                  INTEGER,
    IdIntervento             INTEGER    NOT NULL,
    Istante                  DATETIME   NULL,
    TipoCurva                TEXT       NULL,
    Hs                       FLOAT (53) NULL,
    HsCorretta               FLOAT (53) NULL,
    HsFattoreCorrezione      FLOAT (53) NULL,
    MassaVolumica            FLOAT (53) NULL,
    DensitaRelativa          FLOAT (53) NULL,
    DensitaCorretta          FLOAT (53) NULL,
    DensitaFattoreCorrezione FLOAT (53) NULL,
    Vc                       FLOAT (53) NULL,
    ErrV                     FLOAT (53) NULL,
    ErrT                     FLOAT (53) NULL,
    ErrP                     FLOAT (53) NULL,
    ErrC                     FLOAT (53) NULL,
    ErrZ                     FLOAT (53) NULL,
    C                        FLOAT (53) NULL,
    RoMol                    FLOAT (53) NULL,
    Bmix                     FLOAT (53) NULL,
    xH2                      FLOAT (53) NULL,
    xN2                      FLOAT (53) NULL,
    xCO2                     FLOAT (53) NULL,
    xCH4                     FLOAT (53) NULL,
    T1                       FLOAT (53) NULL,
    T2                       FLOAT (53) NULL,
    Tbase                    FLOAT (53) NULL,
    Pbase                    FLOAT (53) NULL,
    Pmax                     FLOAT (53) NULL,
    Pmin                     FLOAT (53) NULL,
    Tmax                     FLOAT (53) NULL,
    Tmin                     FLOAT (53) NULL,
    VcRead                   FLOAT (53) NULL,
    CRead                    FLOAT (53) NULL,
    VRead                    FLOAT (53) NULL,
    TRead                    FLOAT (53) NULL,
    ZRead                    FLOAT (53) NULL,
    PRead                    FLOAT (53) NULL,
    Pdiff                    FLOAT (53) NULL,
    PambRead                 FLOAT (53) NULL,
    PdiffRead                FLOAT (53) NULL,
    Vmis                     FLOAT (53) NULL,
    Pamb                     FLOAT (53) NULL,
    Pmis                     FLOAT (53) NULL,
    Tmis                     FLOAT (53) NULL,
    Zbase                    FLOAT (53) NULL,
    Z                        FLOAT (53) NULL,
    Fpvb                     FLOAT (53) NULL,
    FpVmis                   FLOAT (53) NULL,
    FpvRead                  FLOAT (53) NULL,
    ZReadDivZb               FLOAT (53) NULL,
    MassaVolumicaAria        FLOAT (53) NULL,
    Methane                  FLOAT (53) NULL,
    Nitrogen                 FLOAT (53) NULL,
    CarbonDioxide            FLOAT (53) NULL,
    Ethane                   FLOAT (53) NULL,
    Propane                  FLOAT (53) NULL,
    Water                    FLOAT (53) NULL,
    HydrogenSulfide          FLOAT (53) NULL,
    Hydrogen                 FLOAT (53) NULL,
    CarbonMonoxide           FLOAT (53) NULL,
    Oxygen                   FLOAT (53) NULL,
    Iso_Butane               FLOAT (53) NULL,
    N_Butane                 FLOAT (53) NULL,
    Iso_Pentane              FLOAT (53) NULL,
    N_Pentane                FLOAT (53) NULL,
    N_Hexane                 FLOAT (53) NULL,
    N_Heptane                FLOAT (53) NULL,
    N_Octane                 FLOAT (53) NULL,
    N_Nonane                 FLOAT (53) NULL,
    N_Decane                 FLOAT (53) NULL,
    Helium                   FLOAT (53) NULL,
    Argon                    FLOAT (53) NULL,
    Fpv                      FLOAT (53) NULL,
    FpvbRead                 FLOAT (53) NULL,
    FpvBase                  FLOAT (53) NULL,
    CONSTRAINT PK_DatiPunto PRIMARY KEY (
        IdPunto
    )
);

-- Table: Interventi
DROP TABLE IF EXISTS Interventi;
CREATE TABLE Interventi (
    IdIntervento                        INTEGER,
    IdTipoIntervento                    NVARCHAR (4)  NULL,
    IdOperatore                         INTEGER       NULL,
    IdTitolare                          INTEGER       NULL,
    IdOrdineDiLavoro                    INTEGER       NULL,
    IdUtenza                            INTEGER       NULL,
    IdDut                               INTEGER       NULL,
    IdContatoreAssociato                INTEGER       NULL,
    IdStatoIntervento                   NVARCHAR (3),
    DataAssegnazione                    DATETIME      NULL,
    DataIntervento                      DATETIME      NULL,
    DataRapporto                        DATETIME      NULL,
    NumeroRapporto                      NVARCHAR (10),
    IstanteInizioIntervento             DATETIME      NULL,
    IstanteFineIntervento               DATETIME      NULL,
    UmiditaAmbiente                     FLOAT (53)    NULL,
    TemperaturaAmbiente                 FLOAT (53)    NULL,
    PressioneAtmosferica                FLOAT (53)    NULL,
    RiesameDocumentaleENoteSuIntervento TEXT          NULL,
    NoteSuTitolareEUtente               TEXT          NULL,
    NoteSuDutECampioni                  TEXT          NULL,
    AltroPersonaleCoinvolto             TEXT          NULL,
    CONSTRAINT PK_Interventi PRIMARY KEY (
        IdIntervento
    )
);
INSERT INTO Interventi (IdIntervento, IdTipoIntervento, IdOperatore, IdTitolare, IdOrdineDiLavoro, IdUtenza, IdDut, IdContatoreAssociato, IdStatoIntervento, DataAssegnazione, DataIntervento, DataRapporto, NumeroRapporto, IstanteInizioIntervento, IstanteFineIntervento, UmiditaAmbiente, TemperaturaAmbiente, PressioneAtmosferica, RiesameDocumentaleENoteSuIntervento, NoteSuTitolareEUtente, NoteSuDutECampioni, AltroPersonaleCoinvolto) VALUES (1, 'VERP', 2, 3, NULL, 5, 2, NULL, 'CRE', '2026-02-04 21:40:27.6060406', NULL, NULL, '', NULL, NULL, NULL, NULL, NULL, '', '', '', '');

-- Table: NormeUtilizzate
DROP TABLE IF EXISTS NormeUtilizzate;
CREATE TABLE NormeUtilizzate (
    IdNorma     NVARCHAR (10)  NOT NULL,
    Sigla       NVARCHAR (20)  NULL,
    Descrizione NVARCHAR (253) NULL,
    OrdineVisualizzazione INTEGER NULL,
    CONSTRAINT PK_NormeUtilizzate PRIMARY KEY (
        IdNorma
    )
);
INSERT INTO NormeUtilizzate (IdNorma, Sigla, Descrizione, OrdineVisualizzazione) VALUES ('ISO9001', '9001', 'Norma di processo che indica i requisiti per la certificazione dei Sistemi di Gestione per la Qualità', 10);

-- Table: Operatori
DROP TABLE IF EXISTS Operatori;
CREATE TABLE Operatori (
    IdOperatore     INTEGER,
    Cognome         NVARCHAR (50) NULL,
    Nome            NVARCHAR (50) NULL,
    Username        NVARCHAR (12) NULL,
    Password        NVARCHAR (50) NULL,
    Salt            NVARCHAR (20) NULL,
    email           NVARCHAR (60) NULL,
    cellulare       NVARCHAR (30) NULL,
    telefono        NVARCHAR (30) NULL,
    disabilitato    BIT           NULL,
    CONSTRAINT PK_Ispettori PRIMARY KEY (
        IdOperatore
    )
);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (1, 'Verdi', 'Giuseppe', '', '', '', 'opera@google.com', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (2, 'Rossi', 'Graziano', '', '', '', '', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (3, 'Albini', 'Luciano', '', '', '', '', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (4, 'Bruni', 'Carla', '', '', '', '', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (5, 'Bianchi', 'Giovanni', '', '', '', '', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (6, 'Neri', 'Giorgio', '', '', NULL, '', '', '', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (7, 'Rossi', 'Valentino', 'vale46', NULL, NULL, 'vale@urca.com', '321 098765432', '05432109876', 0);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (8, 'Negri', 'Antonio', '', NULL, NULL, '', '', '', NULL);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (9, 'Moro', 'Giovanni', '', NULL, NULL, '', '', '', NULL);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (10, 'Bianco', 'Gerardo', '', NULL, NULL, '', '', '', NULL);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (11, 'Rosso', 'Corvo', '', NULL, NULL, '', '', '', NULL);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (12, 'Rossini', 'Gioacchino', '', NULL, NULL, '', '', '', NULL);
INSERT INTO Operatori (IdOperatore, Cognome, Nome, Username, Password, Salt, email, cellulare, telefono, disabilitato) VALUES (13, 'Giallini', 'Marco', '', NULL, NULL, '', '', '', NULL);

-- Table: Operatori_TipiOperatore
DROP TABLE IF EXISTS Operatori_TipiOperatore;
CREATE TABLE Operatori_TipiOperatore (
    IdOperatore     INTEGER       NOT NULL,
    IdTipoOperatore NVARCHAR (5)  NOT NULL,
    CONSTRAINT PK_Operatori_TipiOperatore PRIMARY KEY (
        IdOperatore,
        IdTipoOperatore
    ),
    FOREIGN KEY (IdOperatore) REFERENCES Operatori (IdOperatore),
    FOREIGN KEY (IdTipoOperatore) REFERENCES TipiOperatore (IdTipoOperatore)
);

-- Table: StatiIntervento
DROP TABLE IF EXISTS StatiIntervento;
CREATE TABLE StatiIntervento (
    IdStatoIntervento     NVARCHAR (3)   NOT NULL
                                         PRIMARY KEY,
    DescrizioneBreve      NVARCHAR (20)  NULL,
    Descrizione           NVARCHAR (100) NULL,
    OrdineVisualizzazione INTEGER        NULL
);

-- Table: TipiApprovazione
DROP TABLE IF EXISTS TipiApprovazione;
CREATE TABLE TipiApprovazione (
    IdTipoApprovazione NVARCHAR (4)   NOT NULL,
    DescrizioneBreve   NVARCHAR (30)  NULL,
    Descrizione        NVARCHAR (253) NULL,
    OrdineVisualizzazione INTEGER NULL,
    CONSTRAINT PK_TipiApprovazione PRIMARY KEY (
        IdTipoApprovazione
    )
);

-- Table: TipiConvertitore
DROP TABLE IF EXISTS TipiConvertitore;
CREATE TABLE TipiConvertitore (
    IdTipoConvertitore NVARCHAR (5)   NOT NULL
                                      PRIMARY KEY,
    DescrizioneBreve   NVARCHAR (40)  NULL,
    Descrizione        NVARCHAR (253) NULL,
    OrdineVisualizzazione INTEGER NULL
);

-- Table: TipiIntervento
DROP TABLE IF EXISTS TipiIntervento;
CREATE TABLE TipiIntervento (
    IdTipoIntervento NVARCHAR (4)   NOT NULL,
    DescrizioneBreve NVARCHAR (40)  NULL,
    Descrizione      NVARCHAR (253) NULL,
    OrdineVisualizzazione INTEGER      NULL,
    CONSTRAINT PK_TipiAttivita PRIMARY KEY (
        IdTipoIntervento
    )
);

-- Table: TipiOperatore
DROP TABLE IF EXISTS TipiOperatore;
CREATE TABLE TipiOperatore (
    IdTipoOperatore  NVARCHAR (5)   NOT NULL,
    DescrizioneBreve NVARCHAR (20)  NULL,
    Descrizione      NVARCHAR (100) NULL,
    OrdineVisualizzazione INTEGER NULL,
    PRIMARY KEY (
        IdTipoOperatore
    )
);

-- Table: TipiStrumento
DROP TABLE IF EXISTS TipiStrumento;
CREATE TABLE TipiStrumento (
    IdTipoStrumento       NVARCHAR (4)   NOT NULL,
    DescrizioneBreve      NVARCHAR (30)  NULL,
    Descrizione           NVARCHAR (253) NULL,
    OrdineVisualizzazione INTEGER        NULL,
    CONSTRAINT PK_StrumentiTipi PRIMARY KEY (
        IdTipoStrumento
    )
);

COMMIT TRANSACTION;
PRAGMA foreign_keys = on;
