using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Projet_final
{
    [TestClass]
    public class UnitTest1
    {
        static MyImage coco = new MyImage(@"C:\Users\Alexandre Zajac\Desktop\Programmation\Prog S4\Projet Info - ZAJAC Alexandre\TD 1\bin\Debug\coco.bmp"); // Image rectangulaire
        static MyImage lena = new MyImage(@"C:\Users\Alexandre Zajac\Desktop\Programmation\Prog S4\Projet Info - ZAJAC Alexandre\TD 1\bin\Debug\lena.bmp"); // Image carrée
        static MyImage girl = new MyImage(@"C:\Users\Alexandre Zajac\Desktop\Programmation\Prog S4\Projet Info - ZAJAC Alexandre\TD 1\bin\Debug\girl.bmp"); // Image dont la largeur n'est pas multiple de 4
        static MyImage regular = new MyImage(800, 1200); // Image créee à partir d'une hauteur et largeur (multiple de 4)
        static MyImage unusual = new MyImage(521, 603); // Image créee à partir d'une hauteur et largeur (non-multiple de 4)
        static MyImage CSV = new MyImage(@"C:\Users\Alexandre Zajac\Desktop\Programmation\Prog S4\Projet Info - ZAJAC Alexandre\TD 1\bin\Debug\Image.csv"); // Image crée par un fichier .csv

        static MyImage[] TabImages = { coco, lena, girl, regular, unusual, CSV }; // On teste pour toutes les caractéristiques possibles d'images

        /// <summary>
        /// Vérifie l'écriture et la lecture d'une image pour chaque constructeur
        /// </summary>
        [TestMethod]
        public void VerificationLectureEtEcriture()
        {
            foreach (MyImage element in TabImages)
            {
                Assert.IsNotNull(element); // On vérifie que les objets sont bien instanciés et donc non nuls
                element.From_Image_To_File("UnitTest.bmp"); // On les écrit en fichier de sortie
                Assert.IsNotNull(new MyImage("UnitTest.bmp")); // On vérifie que l'écriture a marché
            }
        }

        /// <summary>
        /// Cette méthode teste si les attributs ajoutés pour le projet fonctionnent
        /// </summary>
        [TestMethod]
        public void VérifierLesNouveauxAttributs()
        {
            Assert.IsTrue(lena.PropIsAllGrey); // lena est bien en monochrome
            Assert.IsFalse(coco.PropIsAllGrey); // coco n'est pas en gris
            Assert.IsTrue(lena.PropIsASquare); // lena est bien un carré
            Assert.IsFalse(coco.PropIsASquare); // coco n'est pas un carré
            Assert.IsTrue(girl.PropBytesToAdd != 0 && girl.PropBytesToAdd == 2); // en effet, la largeur de l'image girl est 1282 qui n'est pas multiple de 4, d'où l'ajout de 2 bits de remplisage car 1282 * 3 couleurs = 3846 et le prochain multiple de 4 est atteint à 3846 + 2 = 3848
            Assert.IsFalse(regular.PropBytesToAdd != 0); // Cette image a une largeur multiple de 4 donc pas de bits de remplissage
        }

        /// <summary>
        /// On teste les méthodes du TD2 car elles servent de base au projet
        /// </summary>
        [TestMethod]
        public void TestsTD2()
        {
            MyImage cocoUpsize = coco.AgrandirHauteur(20);
            Assert.AreEqual(1.2 * coco.PropHauteur, cocoUpsize.PropHauteur);
            cocoUpsize = coco.AgrandirHauteur(50);
            Assert.AreEqual(1.5 * coco.PropHauteur, cocoUpsize.PropHauteur);
            cocoUpsize = coco.AgrandirHauteur(100);
            Assert.AreEqual(2 * coco.PropHauteur, cocoUpsize.PropHauteur); // Test des aggrandissements hauteur
            
            MyImage lenaUpsize = lena.AgrandirHauteur(20);
            Assert.AreEqual(Math.Ceiling(1.2 * lena.PropHauteur), lenaUpsize.PropHauteur); //Math.Ceiling car 1.2 fois la hauteur ne donne pas un int
            lenaUpsize = lena.AgrandirHauteur(50);
            Assert.AreEqual(1.5 * lena.PropHauteur, lenaUpsize.PropHauteur);
            lenaUpsize = lena.AgrandirHauteur(100);
            Assert.AreEqual(2 * lena.PropHauteur, lenaUpsize.PropHauteur); // Test des aggrandissements largeur

            MyImage cocoDownsize = coco.Retrecir(20);
            Assert.AreEqual(0.8 * coco.PropHauteur, cocoDownsize.PropHauteur);
            cocoDownsize = coco.Retrecir(50);
            Assert.AreEqual(0.5 * coco.PropLargeur, cocoDownsize.PropLargeur); // Test du rétrécissement

            MyImage cocoRotation90 = coco;
            cocoRotation90.Rotation(90);
            cocoRotation90.From_Image_To_File("result.bmp");
            MyImage test = new MyImage("result.bmp");
            Assert.IsTrue(coco.PropHauteur == test.PropLargeur); // true car en tournant de 90 on inverse les dimensions de la hauteur et largeur
            MyImage cocoRotation180 = coco;
            cocoRotation180.Rotation(180);
            cocoRotation180.From_Image_To_File("result.bmp");
            MyImage test2 = new MyImage("result.bmp");
            Assert.IsFalse(coco.PropHauteur == test2.PropHauteur); // false car en tournant de 180 les dimensions de la largeur et hauteur restent les mêmes

            coco.ColorToGreys();
            Assert.IsTrue(coco.PropIsAllGrey); // On teste si coco est bien en nuances de gris

            //On ne peut pas vraiment tester la superposition mais elle marche, essayez-là !
        }

        /* LES TD 3, 4 et 5 ne contiennent pas de fonctions de bases à tester */

        /// <summary>
        /// On teste deux méthodes basiques utiles au projet.
        /// </summary>
        [TestMethod]
        public void TestsUtilities()
        {
            Assert.AreEqual(10, Projet_final.MyImage.PGCD(120, 10)); // On teste la méthode du PGCD
            int val1 = 2;
            int val2 = 548;
            Assert.AreEqual(548, Projet_final.MyImage.Max(val1, val2)); // on test la méthode du max
        }
    }
}
