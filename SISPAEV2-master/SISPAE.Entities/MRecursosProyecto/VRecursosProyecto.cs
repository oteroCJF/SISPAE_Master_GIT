using System;
using System.Collections.Generic;
using System.Text;

namespace Sispae.Entities.MRecursosProyecto
{
    public class VRecursosProyecto
    {
        public int IntegracionId { get;set;}
        public int MesId { get;set;}
        public string Mes { get;set;}
        public int PartidaId { get;set;}
        public string Partida { get;set;}
        public decimal Monto { get;set;}
    }
}
