using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Projet_final
{
    class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
        }

        /// <summary>
        /// Retourne le filtre associé à la demande de l'utilisateur 
        /// </summary>
        /// <param name="KernelName"></param>
        /// <returns></returns>
        static int[,] Get3x3Kernel(string KernelName)
        {
            int[,] res = new int[3, 3];
              
            if (KernelName == "Flou")
            {
                int[,] temp = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res[i, j] = temp[i, j];
                    }
                }
            }
            else if (KernelName == "Contraste")
            {
                int[,] temp = { { 0, -1, 0 }, { -1, 5, -1, }, { 0, -1, 0 } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res[i, j] = temp[i, j];
                    }
                }
            }

            else if (KernelName == "Repoussage")
            {
               int[,] temp = { { -2, -1, 0 }, { -1, 1, 1, }, { 0, 1, 2 } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res[i, j] = temp[i, j];
                    }
                }
            }
            else if (KernelName == "Détection de contours")
            {
                int[,] temp = { { 0, 1, 0 }, { 1, -4, 1, }, { 0, 1, 0 } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res[i, j] = temp[i, j];
                    }
                }
            }
            else if (KernelName == "Renforcement de contours")
            {
                int[,] temp = { { 0, 0, 0 }, { -1, 1, 0, }, { 0, 0, 0 } };
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        res[i, j] = temp[i, j];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Affiche le menu entier
        /// </summary>
        static void ShowMenu()
        {
            bool Restart = true;
            while(Restart)
            {
                string choixfinal = "";
                int choixTd = 0;
                Restart = false;

                #region Introduction
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
                string Welcomemsg = "Bienvenue sur le projet de traitement d'images !";
                string Enter = "Veuillez  appuyer sur entrée pour commencer...";
                Console.SetCursorPosition((Console.WindowWidth - Welcomemsg.Length) / 2, 1);
                Console.WriteLine(Welcomemsg);
                Console.SetCursorPosition((Console.WindowWidth - Enter.Length) / 2, Console.WindowHeight / 2);
                Console.WriteLine(Enter);

                while (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.SetCursorPosition((Console.WindowWidth - Welcomemsg.Length) / 2, 1);
                    Console.WriteLine(Welcomemsg);
                    Console.SetCursorPosition((Console.WindowWidth - Enter.Length) / 2, Console.WindowHeight / 2);
                    Console.WriteLine(Enter);
                    Console.ReadKey();
                }
                #endregion

                #region ChoixTD
                int ligne = 0;
                bool end = false;
                string[] tabTds = { "TD 2 - Manipulations", "TD 3 - Filtres", "TD 4 - Formes et histogrammes", "TD 5 - Innovation" };
                while (!end)
                {
                    Console.Clear();
                    string choixTD = "Veuillez sélectionner sur quel TD vous allez traiter votre image.\n\n\n\n\n";
                    Console.SetCursorPosition((Console.WindowWidth - choixTD.Length) / 2, Console.CursorTop);
                    Console.WriteLine(choixTD);
                    for (int i = 0; i < tabTds.Length; i++)
                    {
                        if (ligne == i)
                        {
                            if (i == tabTds.Length - 1)
                                Console.ForegroundColor = ConsoleColor.Cyan;
                            else
                                Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("  -¤-  ");
                            choixTd = i + 2;
                        }
                        else
                        {
                            Console.Write("     ");
                        }
                        Console.WriteLine(tabTds[i]);
                        if (ligne == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    }
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            end = true;
                            break;
                        case ConsoleKey.UpArrow:
                            ligne--;
                            if (ligne < 0)
                                ligne = tabTds.Length - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            ligne++;
                            if (ligne > tabTds.Length - 1)
                                ligne = 0;
                            break;
                    }
                }
                #endregion

                #region ChoixTraitement
                ligne = 0;
                end = false;
                bool PickImage = true;
                List<string> Parameters = new List<string>();
                switch (choixTd)
                {
                    case 2:
                        Parameters.AddRange(new string[] { "Agrandir", "Rétrecir", "Nuances de gris","Noir et Blanc", "Superposition" });
                        break;
                    case 3:
                        Parameters.AddRange(new string[] { "Repoussage", "Détection de contours", "Flou", "Contraste", "Renforcement de contours"});
                        break;
                    case 4:
                        Parameters.AddRange(new string[] { "Image forme triangle", "Image forme triangle inversé", "Image forme carrée", "Histogrammes" });
                        break;
                    case 5:
                        Parameters.AddRange(new string[] { "Réflexion géometrique", "HDR", "Double Exposition" });
                        break;
                }
                while (!end)
                {
                    Console.Clear();
                    string choixTraitement = "Veuillez sélectionner votre choix de traitement.\n\n\n\n\n";
                    Console.SetCursorPosition((Console.WindowWidth - choixTraitement.Length) / 2, Console.CursorTop);
                    Console.WriteLine(choixTraitement);
                    for (int i = 0; i < Parameters.Count; i++)
                    {
                        if (ligne == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("  -¤-  ");
                            choixfinal = Parameters[i];
                        }
                        else
                        {
                            Console.Write("     ");
                        }
                        Console.WriteLine(Parameters[i]);
                        if (ligne == i)
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    }
                    switch (Console.ReadKey().Key)
                    {
                        case ConsoleKey.Enter:
                            end = true;
                            break;
                        case ConsoleKey.UpArrow:
                            ligne--;
                            if (ligne < 0)
                                ligne = Parameters.Count - 1;
                            break;
                        case ConsoleKey.DownArrow:
                            ligne++;
                            if (ligne > Parameters.Count - 1)
                                ligne = 0;
                            break;
                    }
                }
                if ((choixTd == 4 && choixfinal != "Histogrammes") || (choixTd == 5 && choixfinal != "Réflexion géometrique"))
                    PickImage = false;
                #endregion

                #region ChoixImage
                MyImage Work = new MyImage("Resultat.bmp");
                string filename = "";
                if (PickImage)
                {
                    Console.Clear();
                    string imgChoice = "Veuillez choisir l'image en tapant son nom ainsi que son extension...";
                    string warning = "Attention ne choisissez que des images présentent dans le dossier Debug du projet !";
                    Console.SetCursorPosition((Console.WindowWidth - imgChoice.Length) / 2, 1);
                    Console.WriteLine(imgChoice);
                    Console.SetCursorPosition((Console.WindowWidth - warning.Length) / 2, 3);
                    Console.WriteLine(warning);
                    string[] filenamesPossible = { "bear(W).bmp", "bird(W).bmp", "china(DE).bmp", "circle tree.bmp", "coco.bmp", "fogforest(DE).bmp", "girl.bmp","lac_en_montagne.bmp", "italia.bmp","lion(W).bmp", "lena.bmp","lakeReflection(DE).bmp","men(W).bmp", "mountains.bmp", "Mystic(DE).bmp", "MysticForest(DE).bmp", "lake.bmp", "lake+2.bmp", "lake-2.bmp", "lake-4.bmp", "trees(DE).bmp", "treeRoute.bmp", "Paris.bmp", "Paris+2.bmp", "Paris+4.bmp", "Paris-2.bmp","rose.bmp", "rose+2.bmp", "rose+4.bmp", "rose-2.bmp", "skyscrappers(DE).bmp", "volcano(DE).bmp", "volcano2.bmp","women(W).bmp" };
                    filename = Console.ReadLine();
                    bool Validate = false;
                    int compteur = 0;
                    while (!Validate)
                    {
                        if (compteur != 0)
                        {
                            Console.Clear();
                            string Warning = "Le nom ne correspond à aucune image dans le dossier, rééssayez !";
                            Console.SetCursorPosition((Console.WindowWidth - Warning.Length) / 2, 3);
                            Console.WriteLine(Warning);
                            filename = Console.ReadLine();
                        }
                        for (int i = 0; i < filenamesPossible.Length; i++)
                        {
                            if (filename == filenamesPossible[i])
                            {
                                Validate = true;
                                break;
                            }
                        }
                        compteur++;
                    }
                    Work = new MyImage(filename);
                    Console.Clear();
                    if (Work.PropLargeur >= 1000 || Work.PropHauteur >= 1000)
                        Console.WriteLine("L'image est assez grande, il se peut que certains traitement prennent jusqu'à quelques secondes...");
                }

                #endregion

                #region Traitement
                Console.Clear();
                if (choixTd == 2)
                {
                    switch (choixfinal)
                    {
                        case "Agrandir":
                            Console.WriteLine("Choisissez votre pourcentage d'agrandissement de la hauteur multiple de 20, 50 ou 100 % !\n\n");
                            int Phauteur = int.Parse(Console.ReadLine());
                            while(Phauteur % 20 != 0 && Phauteur % 50 != 0)
                            {
                                Console.Write("Cette agrandissement est impossible, veuillez choisir un pourcentage d'agrandissement de la hauteur multiple de 20, 50 ou 100 % !\n\n");
                                Phauteur = int.Parse(Console.ReadLine());
                            }
                            if(Phauteur != 0)
                            {
                                while (Phauteur >= 100)
                                {
                                    Work = Work.AgrandirHauteur(100);
                                    Phauteur -= 100;
                                }
                                if (Phauteur == 50)
                                    Work = Work.AgrandirHauteur(50);
                                else
                                    while(Phauteur >= 20)
                                    {
                                        Work = Work.AgrandirHauteur(20);
                                        Phauteur -= 20;
                                    }
                            }
                            Console.WriteLine("Choisissez votre pourcentage d'agrandissement de la largeur (20, 50 ou 100 %) !\n\n");
                            int Plargeur = int.Parse(Console.ReadLine());
                            while (Plargeur % 20 != 0 && Plargeur % 50 != 0)
                            {
                                Console.Write("Cette agrandissement est impossible, veuillez choisir votre pourcentage d'agrandissement de la largeur (20, 50 ou 100 %) !\n\n");
                                Plargeur = int.Parse(Console.ReadLine());
                            }
                            if (Plargeur != 0)
                            {
                                while (Plargeur >= 100)
                                {
                                    Work = Work.AgrandirLargeur(100);
                                    Plargeur -= 100;
                                }
                                if (Plargeur == 50)
                                    Work = Work.AgrandirLargeur(50);
                                else
                                    while (Plargeur >= 20)
                                    {
                                        Work = Work.AgrandirLargeur(20);
                                        Plargeur -= 20;
                                    }
                            }
                            Work.From_Image_To_File("Resultat.bmp");
                            break;
                        case "Rétrecir":
                            Console.WriteLine("Choisissez votre pourcentage de rétrécissement (20 ou 50 %) !\n\n");
                            int pourcentage = int.Parse(Console.ReadLine());
                            while(pourcentage != 20 && pourcentage != 50)
                            {
                                Console.WriteLine("Ce rétrécissement est impossible, veuillez choisir votre pourcentage de rétrécissement (20 ou 50 %) !\n\n");
                                pourcentage = int.Parse(Console.ReadLine());
                            }
                            Work = Work.Retrecir(pourcentage);
                            Work.From_Image_To_File("Resultat.bmp");
                            break;
                        case "Nuances de gris":
                            Work.ColorToGreys();
                            Work.From_Image_To_File("Resultat.bmp");
                            break;
                        case "Noir et Blanc":
                            Work.NoirEtBlanc();
                            Work.From_Image_To_File("Resultat.bmp");
                            break;
                        case "Superposition":
                            Console.Clear();
                            string imgChoice = "Veuillez choisir la deuxième image en tapant son nom ainsi que son extension...";
                            string warning = "Attention ne choisissez que des images présentent dans le dossier Debug du projet !";
                            Console.SetCursorPosition((Console.WindowWidth - imgChoice.Length) / 2, 1);
                            Console.WriteLine(imgChoice);
                            Console.SetCursorPosition((Console.WindowWidth - warning.Length) / 2, 3);
                            Console.WriteLine(warning);
                            string[] filenamesPossible = { "bear(W).bmp", "bird(W).bmp", "china(DE).bmp", "circle tree.bmp", "coco.bmp", "fog forest(DE).bmp", "lac_en_montagne.bmp", "italia.bmp", "lena.bmp", "mountains.bmp", "Mystic(DE).bmp", "MysticForest(DE).bmp", "lake.bmp", "lake+2.bmp", "lake-2.bmp", "lake-4.bmp", "trees(DE).bmp", "treeRoute.bmp", "girl.bmp", "lion(W).bmp", "men(W).bmp", "lakeReflection(DE).bmp", "Paris.bmp", "Paris+2.bmp", "Paris+4.bmp", "Paris-2.bmp", "rose.bmp", "rose+2.bmp", "rose+4.bmp", "rose-2.bmp", "skyscrappers(DE).bmp", "volcano(DE).bmp", "volcano2.bmp", "women(W).bmp" };
                            string Filename = Console.ReadLine();
                            bool Validate = false;
                            int compteur = 0;
                            while (!Validate)
                            {
                                if (compteur != 0)
                                {
                                    Console.Clear();
                                    string Warning = "Le nom ne correspond à aucune image dans le dossier, rééssayez !";
                                    Console.SetCursorPosition((Console.WindowWidth - Warning.Length) / 2, 3);
                                    Console.WriteLine(Warning);
                                    Filename = Console.ReadLine();
                                }
                                for (int i = 0; i < filenamesPossible.Length; i++)
                                {
                                    if (Filename == filenamesPossible[i])
                                    {
                                        Validate = true;
                                        break;
                                    }
                                }
                                compteur++;
                            }
                            MyImage Superpose = new MyImage(Filename);
                            if(Work.PropHauteur >= Superpose.PropHauteur || Work.PropLargeur >= Superpose.PropLargeur)
                                Work.Superposition(Filename).From_Image_To_File("Resultat.bmp");
                            else
                                Superpose.Superposition(filename).From_Image_To_File("Resultat.bmp");
                            break;
                    }
                    Process.Start("Resultat.bmp");
                }

                else if (choixTd == 3)
                {
                    int[,] kernel = Get3x3Kernel(choixfinal);
                    Work.ApplyFilter(kernel);
                    Work.From_Image_To_File("Resultat.bmp");
                    Process.Start("Resultat.bmp");
                }

                else if (choixTd == 4)
                {
                    if (choixfinal != "Histogrammes")
                    {
                        Console.WriteLine("Choisissez votre hauteur ! (les dimensions doivent respecter : 2 x largeur > hauteur)\n\n");
                        int hauteur = int.Parse(Console.ReadLine());
                        Console.WriteLine("Choisissez votre largeur !\n\n");
                        int largeur = int.Parse(Console.ReadLine());
                        while(2*largeur <= hauteur)
                        {
                            Console.Write("Vous ne respectez pas les conditions ! Veuillez choisir à nouveau.\n\n");
                            Console.WriteLine("Choisissez votre hauteur ! (les dimensions doivent respecter : 2 x largeur > hauteur)\n\n");
                            hauteur = int.Parse(Console.ReadLine());
                            Console.WriteLine("Choisissez votre largeur !\n\n");
                            largeur = int.Parse(Console.ReadLine());
                        }
                        MyImage res = new MyImage(hauteur, largeur);
                        switch (choixfinal)
                        {
                            case "Image forme triangle":
                                res.FormeTriangle();
                                break;
                            case "Image forme triangle inversé":
                                res.FormeTriangle();
                                res.Rotation(180);
                                break;
                            case "Image forme carrée":
                                res.FormeCarree();
                                break;
                        }
                        res.From_Image_To_File("Resultat.bmp");
                        Process.Start("Resultat.bmp");
                    }
                    else
                    {
                        Work.ColorsToHistograms();
                    }
                }

                else
                {
                    switch (choixfinal)
                    {
                        case "Réflexion géometrique":
                            Console.WriteLine("Choisissez votre forme (carré, triangle ou triangle inversé) !");
                            string forme = Console.ReadLine();
                            while(forme != "carré" && forme != "triangle" && forme != "triangle inversé")
                            {
                                Console.WriteLine("Ce n'est pas possible ! Veuillez choisir une forme conforme");
                                forme = Console.ReadLine();
                            }
                            Work.GeometricReflection(forme);
                            Work.From_Image_To_File("Resultat.bmp");
                            Process.Start("Resultat.bmp");
                            break;
                        case "HDR":
                            Console.WriteLine("Choisissez votre suite d'images à différentes exposition (Paris, rose ou lake) !");
                            string choix = Console.ReadLine();
                            while(choix != "Paris" && choix != "rose" && choix != "lake")
                            {
                                Console.WriteLine("Ce n'est pas possible, veuillez choisir la bonne suite d'images !");
                                choix = Console.ReadLine();
                            }
                            if(choix =="Paris")
                            {
                                string[] tab = { "Paris-2.bmp", "Paris.bmp", "Paris+2.bmp", "Paris+4.bmp" };
                                Console.Clear();
                                Console.WriteLine("L'HDR est un processus qui peut durer jusqu'à 1 minute étant donné la taille des images...");
                                ShowHDR(tab);
                            }
                            else if(choix == "rose")
                            {
                                string[] tab = { "rose-2.bmp", "rose.bmp", "rose+2.bmp", "rose+4.bmp" };
                                Console.Clear();
                                Console.WriteLine("L'HDR est un processus qui peut durer jusqu'à 1 minute étant donné la taille des images...");
                                ShowHDR(tab);
                            }
                            else if(choix == "lake")
                            {
                                string[] tab = { "lake-4.bmp", "lake-2.bmp", "lake.bmp", "lake+2.bmp" };
                                Console.Clear();
                                Console.WriteLine("L'HDR est un processus qui peut durer jusqu'à 1 minute étant donné la taille des images...");
                                ShowHDR(tab);
                            }
                            break;
                        case "Double Exposition":
                            Console.WriteLine("Choisissez votre image qui servira de remplissage (label DE) !");
                            string item = Console.ReadLine();
                            while(item[item.Length-6] != 'E')
                            {
                                Console.WriteLine("Ce n'est pas une image autorisée pour ce traitement !");
                                item = Console.ReadLine();
                            }
                            Console.WriteLine("Choisissez votre image qui servira de masque (sur fond blanc) !");
                            string mask = Console.ReadLine();
                            while (mask[mask.Length - 6] != 'W')
                            {
                                Console.WriteLine("Ce n'est pas une image autorisée pour ce traitement !");
                                mask = Console.ReadLine();
                            }
                            ShowDoubleExposure(mask, item);
                            break;
                    }
                }
                #endregion

                Console.Clear();
                string endmsg = "Voulez-vous recommencer (en indiquant oui) ? ";
                Console.SetCursorPosition((Console.WindowWidth - endmsg.Length) / 2, 1);
                Console.WriteLine(endmsg);
                if (Console.ReadLine() == "oui")
                    Restart = true;
                else 
                    Console.WriteLine("Merci d'avoir utilisé le projet de traitement d'images, à bientôt !");
            }
            
        }

        /// <summary>
        /// Montre l'innovation "Double exposition"
        /// </summary>
        /// <param name="item"></param>
        /// <param name="mask"></param>
        static void ShowDoubleExposure(string item, string mask)
        {
            MyImage Item = new MyImage(item);
            MyImage landscape = new MyImage(mask);
            MyImage Mask = Item.GetMask();
            int[] Infos = Mask.Infos();
            Mask.Fill(Item, landscape, Infos, "RightToLeft", false); // On montre le remplissage pour les 4 directions
            Mask.PostProcessing();
            Mask.From_Image_To_File("Mask1.bmp");
            Process.Start(item);
            Process.Start(mask);
            Process.Start("Mask1.bmp");
            Mask.Fill(Item, landscape, Infos, "LeftToRight", false);
            Mask.PostProcessing();
            Mask.From_Image_To_File("Mask2.bmp");
            Process.Start("Mask2.bmp");
            Mask.Fill(Item, landscape, Infos, "TopToBottom", true);
            Mask.PostProcessing();
            Mask.From_Image_To_File("Mask3.bmp");
            Process.Start("Mask3.bmp");
            Mask.Fill(Item, landscape, Infos, "BottomToTop", true);
            Mask.PostProcessing();
            Mask.From_Image_To_File("Mask4.bmp");
            Process.Start("Mask4.bmp");
        }

        /// <summary>
        /// Montre l'innovation HDR en fonction de la suite d'images choisie
        /// </summary>
        /// <param name="tableauImages"></param>
        static void ShowHDR(string[] tableauImages)
        {
            MyImage test = new MyImage(tableauImages[0]);
            MyImage test1 = new MyImage(tableauImages[1]);
            MyImage test2 = new MyImage(tableauImages[2]);
            MyImage test3 = new MyImage(tableauImages[3]); 
            MyImage Weight = test.GetOverallWeightMap(0.9);
            MyImage Weight1 = test1.GetOverallWeightMap(1);
            MyImage Weight2 = test2.GetOverallWeightMap(1);
            MyImage Weight3 = test3.GetOverallWeightMap(0.9); // On crée les images caractéristiques

            MyImage[] tab = { Weight, Weight1, Weight2, Weight3 };
            Weight.NormalizeWeight(tab);
            Weight1.NormalizeWeight(tab);
            Weight2.NormalizeWeight(tab);
            Weight3.NormalizeWeight(tab);
            MyImage[] Images = { test, test1, test2, test3 }; // On les normalise

            MyImage Naive = test1.NaiveBlending(tab, Images); 
            Naive.PostProcessing();
            Naive.From_Image_To_File("HDR.bmp");
            test.From_Image_To_File("image1.bmp");
            test1.From_Image_To_File("image2.bmp");
            test2.From_Image_To_File("image3.bmp");
            test3.From_Image_To_File("image4.bmp");
            Process.Start("image1.bmp");
            Process.Start("image2.bmp");
            Process.Start("image3.bmp");
            Process.Start("image4.bmp");
            Process.Start("HDR.bmp");
        }


    }
}
