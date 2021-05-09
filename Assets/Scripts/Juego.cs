using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{   
    class Juego
    {
        public float tiempo;
        public int cantidadErrores;
        public string nombreJuego;

        public Juego(float tiempo, int cantidadErrores, string nombreJuego)
        {
            this.tiempo = tiempo;
            this.cantidadErrores = cantidadErrores;
            this.nombreJuego = nombreJuego;
        }
    }
}
