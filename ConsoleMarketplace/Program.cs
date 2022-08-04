using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMarketplace
{
    class Program
    {

        public static EntityCollection RetornaEstado(IOrganizationService service, string sigla)
        {
            QueryExpression query = new QueryExpression("clf_estados");
            string[] cols = { "clf_name", "clf_sigla", "clf_estadosid" };
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition("clf_sigla", ConditionOperator.Equal, sigla);
            query.ColumnSet = new ColumnSet(cols);
            return service.RetrieveMultiple(query);
        }

        static void Main(string[] args)
        {
            IOrganizationService service = ConnectionFactory.GetService();


            Console.WriteLine("Bem-vindo ao nosso Portal Online!");
            Console.WriteLine("Por favor, informe seu primeiro nome:");
            string nome = Console.ReadLine();
            while (string.IsNullOrEmpty(nome))
            {
                Console.WriteLine("Entrada inválida! Digite novamente seu primeiro nome: ");
                nome = Console.ReadLine();
            }
            Console.WriteLine("Por favor, informe seu sobrenome:");
            string sobrenome = Console.ReadLine();
            while (string.IsNullOrEmpty(sobrenome))
            {
                Console.WriteLine("Entrada inválida! Digite novamente seu sobrenome: ");
                sobrenome = Console.ReadLine();
            }
            //Idade validation
            Console.WriteLine("Por favor, informe sua idade:");
            string idade = Console.ReadLine();
            int intIdade;
            while (string.IsNullOrEmpty(idade))
            {
                Console.WriteLine("Entrada inválida! Digite novamente sua idade: ");
                idade = Console.ReadLine();
            }
            bool isIdadeInt = int.TryParse(idade, out intIdade);
            while (!isIdadeInt || intIdade < 1)
            {
                Console.WriteLine("Entrada inválida! Digite novamente sua idade: ");
                idade = Console.ReadLine();
                isIdadeInt = int.TryParse(idade, out intIdade);
            }

            Console.WriteLine("Por favor, informe seu telefone com DDD:");
            string telefone = Console.ReadLine();
            while (string.IsNullOrEmpty(telefone))
            {
                Console.WriteLine("Entrada inválida! Digite novamente seu telefone com DDD: ");
                telefone = Console.ReadLine();
            }

            //Tipo validation
            Console.WriteLine("Por favor, informe o número do seu tipo de cliente:");
            Console.WriteLine("1 - Cliente Preferencial");
            Console.WriteLine("2 - Cliente Padrão");
            string tipo = Console.ReadLine();
            int intTipo;
            while (string.IsNullOrEmpty(tipo))
            {
                Console.WriteLine("Entrada inválida! Digite novamente o número do seu tipo de cliente: ");
                tipo = Console.ReadLine();
            }
            bool isTipoInt = int.TryParse(tipo, out intTipo);
            while (!isTipoInt || intTipo != 1 && intTipo != 2)
            {
                Console.WriteLine("Entrada inválida! Digite novamente o número do seu tipo de cliente: ");
                tipo = Console.ReadLine();
                isTipoInt = int.TryParse(tipo, out intTipo);
            }
            


            Console.WriteLine("Por favor, informe um número com casas decimais:");
            string retorno = Console.ReadLine();
            while (string.IsNullOrEmpty(retorno))
            {
                Console.WriteLine("Entrada inválida! Digite novamente um número com casas decimais: ");
                retorno = Console.ReadLine();   
            }
            decimal deci = 0.0M;
            bool isRetornoDecimal = decimal.TryParse(retorno,out deci);

            while(!isRetornoDecimal)
            {
                Console.WriteLine("Entrada inválida! Digite novamente um número com casas decimais: ");
                retorno = Console.ReadLine();
                isRetornoDecimal = decimal.TryParse(retorno, out deci);
            }

            //Sigla validation
            Console.WriteLine("Por favor, informe a sigla do seu estado:");
            string sigla = Console.ReadLine().ToUpper();
            var account = RetornaEstado(service, sigla);
            while (account.Entities.Count < 1)
            {
                Console.WriteLine("Entrada inválida! Digite novamente a sigla do seu estado:");
                sigla = Console.ReadLine();
                account = RetornaEstado(service, sigla);
            }
          
            Guid estadoid = (Guid)account[0].Attributes["clf_estadosid"];

            Entity conta = new Entity("account");
            conta["name"] = nome + " " +sobrenome;
            conta["telephone1"] = telefone;
            conta["clf_age"] = intIdade;
            conta["clf_decimal"] = deci;
            conta["accountcategorycode"] = new OptionSetValue(intTipo);
            conta["clf_estado"] = new EntityReference("clf_estados", estadoid);


            Guid accountid = service.Create(conta);

            Console.WriteLine("Obrigado!");
            Console.WriteLine("Você deseja criar um contato para essa conta? (S/N)");
            var criacontato = Console.ReadLine();
            while (string.IsNullOrEmpty(criacontato) || !criacontato.ToUpper().Equals("S") && !criacontato.ToUpper().Equals("N"))
            {
                Console.WriteLine("Entrada inválida! Digite novamente S ou N: ");
                criacontato = Console.ReadLine();
            }
            criacontato = criacontato.ToUpper();

            if (criacontato.Equals("S"))
            { 
                Entity contato = new Entity("contact");
                contato["firstname"] = nome;
                contato["lastname"] = sobrenome;
                contato["telephone1"] = telefone;
                contato["parentcustomerid"] = new EntityReference("account", accountid);
                service.Create(contato);
            }

            Console.WriteLine("Conta criada com sucesso!");
            Console.Read();
        }
    }
}
