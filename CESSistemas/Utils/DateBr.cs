using System;

namespace Promig.Utils {
    /**
     * Classe utilitaria para recuperar data no formato
     * simplificado brasileiro
     */
    class DateBr {

        public static string GetDateBr() {

            //Retornando data formato string
            return DateTime.Now.ToShortDateString();
        }

    }
}
