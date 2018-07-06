using Promig.Model;
using Promig.WS;

namespace Promig.Utils
{
    /*______________________________________________________________________
     |
     |                         CLASSE UTILITÁRIA
     |
     |      Classe utilitária contendo métodos estaticos para manipulação
     |      de endereços.
     |
     */
    class WSAdress {

        public static Adress GetAdress(string cep) {

            try {

                //Instanciando objeto de busca
                AtendeClienteClient ws = new AtendeClienteClient();
                enderecoERP results = ws.consultaCEP(cep);

                //Instanciando endereço de retorno
                Adress adress = new Adress();
                adress.SetStreet(results.end);
                adress.SetNeighborhood(results.bairro);
                adress.SetCity(results.cidade);
                adress.SetUF(results.uf);

                //Retornando endereço
                return adress;

            }
            catch (System.Exception) {

                //Retorna endereço vazio
                return new Adress();
            }
        }

    }
}
