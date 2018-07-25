using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Repositorio.Metodos
{
    public class Abstracao
    {
        public class Pasta
        {
            public string Caminho { get; set; }

            public string Titulo { get; set; }

            public List<Pasta> Pastas { get; set; }

            public List<Arquivos> Arquivos { get; set; }

        }

        public class Arquivos
        {
            public string Caminho { get; set; }

            public string Titulo { get; set; }
        }

        public class Card
        {
            public string Caminho { get; set; }

            public string Titulo { get; set; }

            public string Imagem { get; set; }
        }
    }
}