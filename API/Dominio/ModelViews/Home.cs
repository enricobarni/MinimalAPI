using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPI.Dominio.ModelViews
{
    public struct Home
    {
        public string Mensagem
        {
            get =>
                "Bem Vindo à API de Veículos 'MinimalAPI'! Para acessar a documentação da API, acesse o seguinte link: ";
        }
        public string doc
        {
            get => "/swagger";
        }
    }
}
