using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    enum Couleurs { Noir, Rouge, Vert, Bleu, Jaune }

    class MasterMind {
        public IEnumerable<Couleurs> Secret {
            get; set;
        }

        public Result Check(IEnumerable<Couleurs> guess)
        {
            var secretZippedVersusGuess = Secret.ZipWith(guess, (sc, gc) => new { sc, gc });
            
            var malPlaces = secretZippedVersusGuess.Where(t => t.gc != t.sc);
            var bienPlaces = secretZippedVersusGuess.Where(t => t.gc == t.sc);
            
            var allColors = Enum.GetValues(typeof(Couleurs)).Cast<Couleurs>();
            
            var nbBonneCouleurMalPlaces = allColors.Sum(c => Math.Min(malPlaces.Where(t => t.sc == c).Count(), malPlaces.Where(t => t.gc == c).Count()));
            int nbBienPlaces = bienPlaces.Count();

            return new Result(nbBienPlaces, nbBonneCouleurMalPlaces);
        }

        static void Main(string[] args)
        {
            MasterMind mastermind = new MasterMind();
            mastermind.Secret = new List<Couleurs>() { Couleurs.Rouge, Couleurs.Vert, Couleurs.Bleu, Couleurs.Jaune };

            System.Console.WriteLine(mastermind.Check(new List<Couleurs>() { Couleurs.Rouge, Couleurs.Vert, Couleurs.Bleu, Couleurs.Jaune }));
            System.Console.WriteLine(mastermind.Check(new List<Couleurs>() { Couleurs.Jaune, Couleurs.Bleu, Couleurs.Vert, Couleurs.Rouge }));
            System.Console.WriteLine(mastermind.Check(new List<Couleurs>() { Couleurs.Jaune, Couleurs.Jaune, Couleurs.Jaune, Couleurs.Noir }));
            System.Console.WriteLine(mastermind.Check(new List<Couleurs>() { Couleurs.Jaune, Couleurs.Jaune, Couleurs.Jaune, Couleurs.Jaune }));
            System.Console.Read();
        }
    }

    class Result {
        public Result(int bienPlaces, int malPlaces) {
            BienPlaces = bienPlaces;
            MalPlaces = malPlaces;
        }

        public int BienPlaces {
            get; set;
        }

        public int MalPlaces {
            get; set;
        }

        public override String ToString() {
            return "(" + BienPlaces + "," + MalPlaces + ")";
        }
    }

    static class Extensions {
        public static IEnumerable<R> ZipWith<T1, T2, R>(this IEnumerable<T1> leftList, IEnumerable<T2> rightList, Func<T1, T2, R> zipper) {
            using (var enumLeft = leftList.GetEnumerator()) {
                using (var enumRight = rightList.GetEnumerator()) {
                    while (enumLeft.MoveNext() && enumRight.MoveNext())
                        yield return zipper(enumLeft.Current, enumRight.Current);
                }
            }
        }
    }
}
