using DesignPartners.Behavioral_Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPartners
{
    class Program
    {
        static void Main(string[] args)
        {
            // *** Behavioral Patterns - Padrões comportamentais ***
            // Fábrica Abstrata
            AbstractFactory.MainApp(); // Fornece uma interface para criar famílias de objetos relacionados ou dependentes sem especificar suas classes concretas.
            // Construtora
            Builder.MainApp(); // Separe a construção de um objeto complexo de sua representação para que o mesmo processo de construção possa criar diferentes representações.
            // Método de fábrica
            FactoryMethod.MainApp(); // Defina uma interface para criar um objeto, mas deixe as subclasses decidirem qual classe instanciar. O Factory Method permite que uma classe adie a instanciação para subclasses.
            // Protótipo
            Prototype.MainApp(); // Especifique o tipo de objetos para criar usando uma instância prototípica e crie novos objetos copiando esse protótipo.
        }
    }
}
