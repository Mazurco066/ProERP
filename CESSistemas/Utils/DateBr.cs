using System;

namespace Promig.Utils {
    /**
     * Classe utilitaria para recuperar data no formato
     * simplificado brasileiro
     */
    class DateBr {

        /// <summary>
        /// Método para retornar data no formato brasileiro simplificado
        /// </summary>
        /// <returns></returns>
        public static string GetDateBr() {

            //Retornando data formato string
            return DateTime.Now.ToShortDateString();
        }

    }
}
