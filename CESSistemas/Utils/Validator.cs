﻿namespace Promig.Utils {

    class Validator {

        /**
         * Validação de CPF
         */
        public static bool IsCpf(string cpf) {

            //Definindo critérios
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            //Verificando tamanho e equivalencia do cpf
            if (cpf.Length != 11) return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++) soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++) soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;
            digito = digito + resto.ToString();

            //Retornando Válido ou inválido
            return cpf.EndsWith(digito);

        }

        /**
         * Validação de CNPJ
         */
        public static bool IsCnpj(string cnpj) {

            //Definindo critérios
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            //Verificando se tamanho de digitos equivale
            if (cnpj.Length != 14) return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++) soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);

            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++) soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);

            if (resto < 2) resto = 0;
            else resto = 11 - resto;
            digito = digito + resto.ToString();

            //Retornando válido ou inválido
            return cnpj.EndsWith(digito);

        }

        /**
         * Validação de PIS
         */
        public static bool IsPis(string pis) {

            //Definindo critérios
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            if (pis.Trim().Length != 11)
                return false;
            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            //Verificando equivalencia do pis
            soma = 0;
            for (int i = 0; i < 10; i++) soma += int.Parse(pis[i].ToString()) * multiplicador[i];
            resto = soma % 11;
            if (resto < 2) resto = 0;
            else resto = 11 - resto;

            //Retornando válido ou inválido
            return pis.EndsWith(resto.ToString());

        }

    }
}

