using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comuni
{
    internal class Provincia
    {
        internal int CodRegione;
        internal string Regione;
        internal int CodProvincia;
        internal string SiglaAuto;
        internal string Denominazione;
        internal double G;

        internal Provincia(int CodRegione, string Regione, int CodProvincia, string SiglaAuto,
            string Denominazione, double G)
        {
            this.CodRegione = CodRegione;
            this.Regione = Regione;
            this.CodProvincia = CodProvincia;
            this.SiglaAuto = SiglaAuto;
            this.Denominazione = Denominazione;
            this.G = G;
        }
    }
}
