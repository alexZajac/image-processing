using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Projet_final
{
    public class MyImage
    {
        private string type; // Type du fichier (Bm dans la plupart des cas)
        private int taille; // taille du fichier
        private int tailleOffset; //tailleOffset du fichier
        private int largeur; //largeur de l'image
        private int hauteur; //hauteur de l'image
        private int nbBits; //nombre de bits de profondeur
        private byte[] tabDeBytes; // Tableau de bytes de l'image entière.
        private bool IsASquare = false;  // Teste si l'image est un carrée.
        private bool SwitchDimensions = false; // Booléen qui détermine si l'on doit changer les dimensions de l'image.
        private bool IsAllGrey = true; // Champ qui détermine si l'image est monochrome ou en couleurs.
        private Pixel[,] ImageRGB; // Matrice de Pixels de l'image.
        private int BytesToAdd; // Bits de remplissage pour les images non multiples de 4

        #region Constructeurs

        /// <summary>
        /// Constructeur de la classe MyImage à partir d'un fichier.
        /// </summary>
        /// <param name="myfile"></param>
        public MyImage(string myfile)
        {     
            if (Path.GetExtension(myfile) == ".csv")
            {
                List<int> FileIntegers = BytesDuFichier(myfile);
                tabDeBytes = new byte[FileIntegers.Count];
                for (int i = 0; i < FileIntegers.Count; i++)
                {
                    tabDeBytes[i] = (byte)(FileIntegers[i]);
                }
            }
            else
            {
                tabDeBytes = File.ReadAllBytes(myfile);
            }
            type = "Le type du fichier est " + Convert.ToChar(tabDeBytes[0]).ToString() + " " + Convert.ToChar(tabDeBytes[1]).ToString();
            
            byte[] TabTaille = FillConstructor(2);
            taille = ConvertirEndianToInt(TabTaille);

            byte[] TabTailleOffset = FillConstructor(14);
            tailleOffset = ConvertirEndianToInt(TabTailleOffset);
            
            byte[] TabLargeur = FillConstructor(18);
            largeur = ConvertirEndianToInt(TabLargeur);
            
            byte[] TabHauteur = FillConstructor(22);
            hauteur = ConvertirEndianToInt(TabHauteur);

            byte[] TabNbBits = new byte[2]; 
            int n = 28;
            while (n < 30)
            {
                for (int i = 0; i < 2; i++)
                {
                    TabNbBits[i] = tabDeBytes[n];
                    n++;
                }
            }
            nbBits = ConvertirEndianToInt(TabNbBits);
            BytesToAdd = largeur % 4;
            ImageRGB = new Pixel[hauteur, largeur];
            int manipulation = 0;
            int o = 54;
            for (int ligne = 0; ligne < hauteur; ligne++)
            {
                manipulation = 0;
                for (int colonne = 0; colonne < largeur; colonne++)
                {
                    if (o < tabDeBytes.Length)
                    {
                        ImageRGB[ligne, colonne] = new Pixel(tabDeBytes[o+2], tabDeBytes[o + 1], tabDeBytes[o]);
                        if (tabDeBytes[o] != tabDeBytes[o + 1] || tabDeBytes[o + 1] != tabDeBytes[o + 2] || tabDeBytes[o] != tabDeBytes[o + 2])
                            IsAllGrey = false;
                        o += 3;
                        manipulation++;
                        if(manipulation==largeur && BytesToAdd != 0)
                            o += BytesToAdd;
                       
                    }
                }               
            }
            if (hauteur == largeur) { IsASquare = true; }
            
        }

        /// <summary>
        /// Constructeur de la classe MyImage à partir d'une hauteur et largeur.
        /// </summary>
        /// <param name="Hauteur"></param>
        /// <param name="Largeur"></param>
        public MyImage(int Hauteur, int Largeur)
        {
            if (Hauteur == Largeur) { IsASquare = true; }
            hauteur = Hauteur;
            largeur = Largeur;
            BytesToAdd = largeur % 4;
            tabDeBytes = new byte[(hauteur * (largeur+BytesToAdd) * 3) + 54];
            byte[] taille = ConvertirIntToEndian((Hauteur * Largeur * 3) + 54);
            byte[] HauteurEnLittleEndian = ConvertirIntToEndian(hauteur);
            byte[] LargeurEnLittleEndian = ConvertirIntToEndian(largeur);
            byte[] TailleImage = ConvertirIntToEndian(Hauteur * Largeur * 3);
            byte[] Infos = { 66, 77, taille[0], taille[1], taille[2], taille[3], 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, LargeurEnLittleEndian[0], LargeurEnLittleEndian[1], LargeurEnLittleEndian[2], LargeurEnLittleEndian[3], HauteurEnLittleEndian[0], HauteurEnLittleEndian[1], HauteurEnLittleEndian[2], HauteurEnLittleEndian[3], 1, 0, 24, 0, 0, 0, 0, 0, TailleImage[0], TailleImage[1], TailleImage[2], TailleImage[3], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            
            ImageRGB = new Pixel[Hauteur, Largeur];
            for (int ligne = 0; ligne < Hauteur; ligne++)
            {
                for (int colonne = 0; colonne < Largeur; colonne++)
                {
                    ImageRGB[ligne, colonne] = new Pixel(255, 255, 255);
                }
            }
            for (int j = 0; j < 54; j++)
            {
                tabDeBytes[j] = Infos[j];
            }
        }

        #endregion

        #region Propriétés

        public string PropType
        {
            get { return type; }
        }

        public bool PropIsAllGrey
        {
            get { return IsAllGrey; }
        }

        public bool PropIsASquare
        {
            get { return IsASquare; }
        }

        public bool PropSwitchDimensions
        {
            get { return SwitchDimensions; }
        }

        public int PropBytesToAdd
        {
            get { return BytesToAdd; }
        }

        public byte[] PropTabdeBytes
        {
            get { return tabDeBytes; }
            set { tabDeBytes = value; }
        }

        public int PropTaille
        {
            get
            {
                return taille;
            }
        }

        public int PropTailleOffset
        {
            get
            {
                return tailleOffset;
            }
        }

        public int PropLargeur
        {
            get {
                return largeur;
            }
            set { largeur = value; }
        }

        public int PropHauteur
        {
            get
            {
                return hauteur;
            }
            set { hauteur = value; }
        }

        public int PropNbBits
        {
            get
            {
                return nbBits;
            }
        }

        public bool Switch
        {
            get { return SwitchDimensions; }
            set { SwitchDimensions = value; }
        }

        public Pixel[,] PropImageRGB
        {
            get
            {
                return ImageRGB;
            }
            set
            {
                ImageRGB = value;
            }
        }

        #endregion

        #region TD 1

        /// <summary>
        /// Cette méthode convertit un tableau d'endian format little endian en entier.
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int ConvertirEndianToInt(byte[] tab)
        {
            int entier = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                entier += (int)(tab[i] * Math.Pow(256, i)); //l'entier est égal à la somme des puissance de 256 multiplié par les valeurs du tableau.
            }
            return entier;
        }

        /// <summary>
        /// Cette méthode convertit un entier en tableau de bytes format little Endian.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public byte[] ConvertirIntToEndian(int number)
        {
            byte[] res = new byte[4];
            res[3] = (byte)(number / Math.Pow(256, 3));
            for (int i = res.Length - 1; i > 0; i--) // on effectue le raisonnement inverse de la conversion d'un endian en entier.
            {
                if (res[i] == 0) { res[i - 1] = (byte)(number / Math.Pow(256, i - 1)); }
                else { res[i - 1] = (byte)(number - res[i] * Math.Pow(256, i)); }
            }
            return res;
        }

        /// <summary>
        /// Cette méthode écrit dans un fichier de sortie différent du fichier d'entrée l'image modifiée.
        /// </summary>
        /// <param name="file"></param>
        public void From_Image_To_File(string file)
        {
            GetOutputTab(); // On appelle la méthode qui retourne le tableau de Bytes de l'image modifié à partir de la matrice de pixels.
            if (SwitchDimensions)
            {
                ChangeHeightWidth(largeur, hauteur); // On inverse largeur et hauteur si l'attribut SwitchDimension est vrai (par exemple pour une rotation). 
            }
            File.WriteAllBytes(file, tabDeBytes); // On écrit dans le fichier de sortie.
        }

        /// <summary>
        /// Cette méthode obtient le tableau de bytes modifié à partir de la matrice de Pixels.
        /// </summary>
        public void GetOutputTab()
        {
            int index1 = 0;
            int index2 = 0;
            for (int colonne = 54; colonne < tabDeBytes.Length && index1 < ImageRGB.GetLength(0); colonne++) // On ne traite que la partie après le header.
            {
                tabDeBytes[colonne+2] = ImageRGB[index1, index2].Red;
                tabDeBytes[colonne + 1] = ImageRGB[index1, index2].Green;
                tabDeBytes[colonne] = ImageRGB[index1, index2].Blue;
                index2++;
                colonne += 2;
                if (index2 == largeur) // si nous sommes à la fin de la largeur de l'image.
                {
                    if(BytesToAdd != 0) // S'il y a besoin de bits de remplissage.
                    {
                        for (int i = colonne+1; i < BytesToAdd + colonne+1; i++)
                        {
                            tabDeBytes[i] = 0; // on ajoute les bits.
                        }
                        colonne += BytesToAdd;
                    }
                    index1++;
                    index2 = 0; //On passe à la aligne suivante.
                }
            }
        }

        /// <summary>
        /// Change la hauteur et largeur de l'image dans le header.
        /// </summary>
        /// <param name="NewHeight"></param>
        /// <param name="NewWidth"></param>
        public void ChangeHeightWidth(int NewHeight, int NewWidth)
        {
            byte[] TabLargeur = ConvertirIntToEndian(NewWidth);
            byte[] TabHauteur = ConvertirIntToEndian(NewHeight);
            int l = 18;
            while (l < 22)
            {
                for (int i = 0; i < 4; i++)
                {
                    tabDeBytes[l] = TabLargeur[i];
                    l++;
                }
            }
            int m = 22;
            while (m < 26)
            {
                for (int i = 0; i < 4; i++)
                {
                    tabDeBytes[m] = TabHauteur[i];
                    m++;
                }
            }
        }

        #endregion

        #region TD 2

        /// <summary>
        /// Agrandit la hauteur de l'image.
        /// </summary>
        /// <param name="PourcentHauteur"></param>
        /// <returns></returns>
        public MyImage AgrandirHauteur(int PourcentHauteur)
        {
            double RatioHauteur = 1 + (double)PourcentHauteur / 100;
            int NouvelleHauteur = (int)Math.Ceiling((RatioHauteur * hauteur)); // On arrondi à l'entier supérieur par convention.
            int[] SmallestFracHeight = GetSmallestFraction(NouvelleHauteur, hauteur); // On calcule la petite fraction entre la nouvelle hauteur et la hauteur d'origine.
            MyImage res = new MyImage(NouvelleHauteur, largeur); 
            int offsetHeight = 0; // Décalage colonnes nécéssaire pour l'agrandissement.
            for (int i = 0; i < res.PropHauteur; i++)
            {
                for (int j= 0; j < largeur; j++)
                {
                    if ((i + 1) % (SmallestFracHeight[0]) == 0) // Si le pixel à l'index suivant doit être modifié
                    {
                        if (i == res.PropImageRGB.GetLength(0) - 1)
                        {
                            res.PropImageRGB[i, j] = ImageRGB[i - offsetHeight - 1, j];
                            offsetHeight = (i + 1) / SmallestFracHeight[0];
                        }
                        else // on affecte à chaque couleur la moyenne des deux pixels adjacents.
                        {
                            offsetHeight = (i + 1) / SmallestFracHeight[0]; // on incrémente le décalage.
                            res.PropImageRGB[i, j].Red = (byte)((ImageRGB[i - offsetHeight, j].Red + ImageRGB[i - offsetHeight + 1, j].Red) / 2);
                            res.PropImageRGB[i, j].Green = (byte)((ImageRGB[i - offsetHeight, j].Green + ImageRGB[i - offsetHeight + 1, j].Green) / 2);
                            res.PropImageRGB[i, j].Blue = (byte)((ImageRGB[i - offsetHeight, j].Blue + ImageRGB[i - offsetHeight + 1, j].Blue) / 2);
                        }
                    }
                    else // Sinon on affecte la valeur de l'ancienne matrice en prenant compte le décalage.
                    {
                        while (i - offsetHeight >= hauteur)
                            offsetHeight++;  // On vérifie que l'on est pas hors limites car pour un grand nombre d'itérations, la hauteur pourrait être un double et faire sortir l'index du tableau (offset trop petit).
                        res.PropImageRGB[i, j] = ImageRGB[i - offsetHeight, j];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Agrandit la largeur de l'image
        /// </summary>
        /// <param name="PourcentLargeur"></param>
        /// <returns></returns>
        public MyImage AgrandirLargeur(int PourcentLargeur)
        {
            double RatioLargeur = 1 + (double)PourcentLargeur / 100;
            int NouvelleLargeur = (int)Math.Ceiling((RatioLargeur * largeur)); // On arrondi à l'entier supérieur par convention.
            int[] SmallestFracWidth = GetSmallestFraction(NouvelleLargeur, largeur);// On calcule la petite fraction entre la nouvelle hauteur et la hauteur d'origine.
            MyImage res = new MyImage(hauteur, NouvelleLargeur);
            int offsetWidth;  // Décalage colonnes nécéssaire pour l'agrandissement.
            for (int i = 0; i < hauteur; i++)
            {
                offsetWidth = 0;
                for (int j = 0; j < res.PropLargeur; j++)
                {
                    if ((j + 1) % SmallestFracWidth[0] == 0)// Si le pixel à l'index suivant doit être modifié
                    {
                        if (j == res.PropImageRGB.GetLength(1) - 1)
                        {
                            res.PropImageRGB[i, j] = ImageRGB[i, j - offsetWidth - 1];
                            offsetWidth++;
                        }
                        else // on affecte à chaque couleur la moyenne des deux pixels adjacents.
                        {
                            offsetWidth = (j + 1) / SmallestFracWidth[0];
                            res.PropImageRGB[i, j].Red = (byte)((ImageRGB[i, j - offsetWidth].Red + ImageRGB[i, j - offsetWidth + 1].Red) / 2);
                            res.PropImageRGB[i, j].Green = (byte)((ImageRGB[i, j - offsetWidth].Green + ImageRGB[i, j - offsetWidth + 1].Green) / 2);
                            res.PropImageRGB[i, j].Blue = (byte)((ImageRGB[i, j - offsetWidth].Blue + ImageRGB[i, j - offsetWidth + 1].Blue) / 2);
                        }
                    }
                    else // Sinon on affecte la valeur de l'ancienne matrice en prenant compte le décalage.
                    {
                        while (j - offsetWidth >= largeur)
                            offsetWidth++; // On vérifie que l'on est pas hors limites car pour un grand nombre d'itérations, la largeur pourrait être un double et faire sortir l'index du tableau (offset trop petit).
                        res.PropImageRGB[i, j] = ImageRGB[i , j - offsetWidth];
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Retourne la plus petite fraction entre deux nombre sous la forme d'un tableau.
        /// </summary>
        /// <param name="NewDimension"></param>
        /// <param name="OldDimension"></param>
        /// <returns></returns>
        public int[] GetSmallestFraction(int NewDimension, int OldDimension)
        {
            int[] res = new int[2]; // Numerateur et denominateur de la plus petite fraction correspondant au double
            double ratio = (double)NewDimension / OldDimension;
            double temp = 10 * ratio;
            double Diviseur = PGCD((int)temp, 10);
            if (Diviseur == 1) { res[0] = (int)temp; res[1] = 10; }
            else
            {
                res[0] = (int)(temp / Diviseur);
                res[1] = (int)(10 / Diviseur);
            }
            return res;
        }

        /// <summary>
        /// Rétrécit l'image en gardant le même ratio. (20 ou 50%)
        /// </summary>
        /// <param name="Pourcent"></param>
        /// <returns></returns>
        public MyImage Retrecir(int Pourcent) 
        {
            double Ratio = 1 - (double)Pourcent / 100;
            int[] SmallestFrac = GetSmallestFraction((int)(Ratio * largeur), largeur);
            MyImage res = new MyImage((int)(hauteur * Ratio), (int)(largeur * Ratio));
            int offsetWidth; //décalage largeur
            int offsetHeight = 0; //décalage hauteur
            for (int i = 0; i < res.PropImageRGB.GetLength(0); i++)
            {
                offsetWidth = 0;
                if ((i + 1) % (SmallestFrac[0]) == 0) // On vérfie si la hauteur doit être rétrécie à ce pixel.
                {
                    for (int j = 0; j < res.PropImageRGB.GetLength(1); j++)
                    {
                        if ((j + 1) % (SmallestFrac[0]) == 0 && j != 0) { offsetWidth++; } // On incrémente le décalage de largeur si largeur doit être modifié pour ce pixel.
                        if (i == res.PropImageRGB.GetLength(0) - 1)
                        {
                            res.PropImageRGB[i, j] = ImageRGB[i + offsetHeight - 1, j + offsetWidth];
                            offsetHeight = (i + 1) / SmallestFrac[0];
                        }
                        else // on affecte à chaque couleur la moyenne des deux pixels adjacents.
                        {
                            offsetHeight = (i + 1) / SmallestFrac[0];
                            res.PropImageRGB[i, j].Red = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Red + ImageRGB[i + offsetHeight + 1, j + offsetWidth].Red) / 2);
                            res.PropImageRGB[i, j].Green = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Green + ImageRGB[i + offsetHeight + 1, j + offsetWidth].Green) / 2);
                            res.PropImageRGB[i, j].Blue = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Blue + ImageRGB[i + offsetHeight + 1, j + offsetWidth].Blue) / 2);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < res.PropImageRGB.GetLength(1); j++)
                    {
                        if ((j + 1) % (SmallestFrac[0]) == 0 && j != 0) // On vérfie si la largeur doit être rétrécie à ce pixel.
                        {
                            if (j == res.PropImageRGB.GetLength(1) - 1)
                            {
                                res.PropImageRGB[i, j] = ImageRGB[i + offsetHeight, j + offsetWidth - 1];
                                offsetWidth++;
                            }
                            else
                            {
                                offsetWidth++;
                                res.PropImageRGB[i, j].Red = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Red + ImageRGB[i + offsetHeight, j + offsetWidth + 1].Red) / 2);
                                res.PropImageRGB[i, j].Green = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Green + ImageRGB[i + offsetHeight, j + offsetWidth + 1].Green) / 2);
                                res.PropImageRGB[i, j].Blue = (byte)((ImageRGB[i + offsetHeight, j + offsetWidth].Blue + ImageRGB[i + offsetHeight, j + offsetWidth + 1].Blue) / 2);
                            }
                        }
                        else // Si on ne doit pas rétrécir à cet endroit alors on affecte au pixel la valeur en prenant compte le décalage.
                        {
                            res.PropImageRGB[i, j] = ImageRGB[i + offsetHeight, j + offsetWidth];
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Tourne l'image de 90, 180 ou 270°.
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation(int angle)
        {
            Pixel[,] temp = new Pixel[ImageRGB.GetLength(0), ImageRGB.GetLength(1)];
            SetEqualWithCurrent(temp);
            if (angle == 90)
            {
                if (IsASquare) // Si c'est un carré 
                {
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            ImageRGB[i, j] = temp[ImageRGB.GetLength(0) - 1 - j, i]; // On "tourbne" les pixels de 90°.
                        }
                    }
                }
                else // Si c'est un rectangle
                {
                    int offset = 0;
                    int difference = 0;
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            if (offset == ImageRGB.GetLength(0)) { offset = 0; difference++; }
                            ImageRGB[i, j] = temp[ImageRGB.GetLength(0) - 1 - offset, difference];
                            offset++;
                        }
                    }
                    SwitchDimensions = true;
                }
            }
            if (angle == 180)
            {
                if (IsASquare) // Si c'est un carré
                {
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            ImageRGB[i, j] = temp[ImageRGB.GetLength(0) - 1 - i, ImageRGB.GetLength(1) - 1 - j];  // On "tourbne" les pixels de 180°.
                        }
                    }
                }
                else // Si c'est un rectangle
                {
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            ImageRGB[i, j] = temp[ImageRGB.GetLength(0) - 1 - i, ImageRGB.GetLength(1) - 1 - j];
                        }
                    }
                }
            }
            if (angle == 270)
            {
                if (IsASquare) // Si c'est un carré
                {
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            ImageRGB[i, j] = temp[j, ImageRGB.GetLength(1) - 1 - i]; // On "tourbne" les pixels de 270°.
                        } 
                    }
                }
                else // Si c'est un rectangle
                {
                    int offset = 0;
                    int difference = 0;
                    for (int i = 0; i < ImageRGB.GetLength(0); i++)
                    {
                        for (int j = 0; j < ImageRGB.GetLength(1); j++)
                        {
                            if (offset == ImageRGB.GetLength(0)) { offset = 0; difference++; }
                            ImageRGB[i, j] = temp[offset, ImageRGB.GetLength(1) - 1 - difference];
                            offset++;
                        }
                    }
                    SwitchDimensions = true;
                }
            }
        }

        /// <summary>
        /// Retourne une image en nuances de gris
        /// </summary>
        public void ColorToGreys()
        {
            Pixel[,] temp = new Pixel[ImageRGB.GetLength(0), ImageRGB.GetLength(1)];
            SetEqualWithCurrent(temp);
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    byte[] CouleursPixel = { ImageRGB[i, j].Red, ImageRGB[i, j].Green, ImageRGB[i, j].Blue };
                    int SommeCouleursPixel = CouleursPixel[0] + CouleursPixel[1] + CouleursPixel[2];
                    if (SommeCouleursPixel % 3 == 0) // etant donné que nous allons effectuer la moyenne, on cherche le reste avec 3 conditions afin d'affecter la bonne valeur.
                    {
                        ImageRGB[i, j].Red = ConvertirIntToEndian(SommeCouleursPixel / 3)[0];
                        ImageRGB[i, j].Green = ImageRGB[i, j].Red;
                        ImageRGB[i, j].Blue = ImageRGB[i, j].Red; // On affecte la même valeur aux 3 pixels.
                    }
                    else if (SommeCouleursPixel % 3 == 1)
                    {
                        ImageRGB[i, j].Red = ConvertirIntToEndian((SommeCouleursPixel - 1) / 3)[0];
                        ImageRGB[i, j].Green = ImageRGB[i, j].Red;
                        ImageRGB[i, j].Blue = ImageRGB[i, j].Red;// On affecte la même valeur aux 3 pixels.
                    }
                    else if (SommeCouleursPixel % 3 == 2)
                    {
                        ImageRGB[i, j].Red = ConvertirIntToEndian((SommeCouleursPixel + 1) / 3)[0];
                        ImageRGB[i, j].Green = ImageRGB[i, j].Red;
                        ImageRGB[i, j].Blue = ImageRGB[i, j].Red;// On affecte la même valeur aux 3 pixels.
                    }
                }
            }
            IsAllGrey = true;
        }

        /// <summary>
        /// Retourne une image en noir et blanc
        /// </summary>
        public void NoirEtBlanc()
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if ((ImageRGB[i, j].Red + ImageRGB[i, j].Green + ImageRGB[i, j].Blue) / 3 >= 128)
                    {
                        ImageRGB[i, j].Red = 255;
                        ImageRGB[i, j].Green = 255;
                        ImageRGB[i, j].Blue = 255;
                    }
                    else
                    {
                        ImageRGB[i, j].Red = 0;
                        ImageRGB[i, j].Green = 0;
                        ImageRGB[i, j].Blue = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Superpose deux images sur une nouvelle image
        /// </summary>
        /// <param name="Image2Path"></param>
        /// <returns></returns>
        public MyImage Superposition(string Image2Path) 
        {
            MyImage SecondImage = new MyImage(Image2Path);
            MyImage res = new MyImage(Max(hauteur, SecondImage.PropHauteur), Max(largeur, SecondImage.PropLargeur)); // Crée une image qui la plus grande largeur et hauteur entre mes deux.

            if (Max(hauteur, SecondImage.PropHauteur) == hauteur && Max(largeur, SecondImage.PropLargeur) == largeur)
            { SetEqualWithCurrent(res.PropImageRGB); } // Si notre image de l'objet courant est la plus grande alors on initialise notre image res avec les pixels de notre objet courant.
            PlaceSecondImage(SecondImage, res); // On place l'image au centre.

            for (int i = 0; i < res.PropImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < res.PropImageRGB.GetLength(1); j++)
                {
                    byte[] CouleursPixel = new byte[3]; // Tableau correspondant aux 3 couleurs du pixel
                    if (i >= ImageRGB.GetLength(0) || j >= ImageRGB.GetLength(1)) // 
                    {
                        CouleursPixel[0] = res.PropImageRGB[i, j].Red;
                        CouleursPixel[1] = res.PropImageRGB[i, j].Green;
                        CouleursPixel[2] = res.PropImageRGB[i, j].Blue;
                    }
                    else
                    {
                        CouleursPixel[0] = ImageRGB[i, j].Red;
                        CouleursPixel[1] = ImageRGB[i, j].Green;
                        CouleursPixel[2] = ImageRGB[i, j].Blue;
                    }

                    int CouleurRouge = CouleursPixel[0] + res.PropImageRGB[i, j].Red; // On additionne les valeurs des pixels des deux images
                    int CouleurVerte = CouleursPixel[1] + res.PropImageRGB[i, j].Green;
                    int CouleurBleue = CouleursPixel[2] + res.PropImageRGB[i, j].Blue;

                    if (CouleurRouge % 2 == 0) { res.PropImageRGB[i, j].Red = ConvertirIntToEndian(CouleurRouge / 2)[0]; } // On s'assure à chaque fois que l'on a la bonne valeur lorsque l'on fais la moyenne des deux pixels.
                    else { res.PropImageRGB[i, j].Red = ConvertirIntToEndian((CouleurRouge / 2) + 1)[0]; }
                    if (CouleurVerte % 2 == 0) { res.PropImageRGB[i, j].Green = ConvertirIntToEndian(CouleurVerte / 2)[0]; }
                    else { res.PropImageRGB[i, j].Green = ConvertirIntToEndian((CouleurVerte / 2) + 1)[0]; }
                    if (CouleurBleue % 2 == 0) { res.PropImageRGB[i, j].Blue = ConvertirIntToEndian(CouleurBleue / 2)[0]; }
                    else { res.PropImageRGB[i, j].Blue = ConvertirIntToEndian((CouleurBleue / 2) + 1)[0]; }
                }
            }
            return res;
        }

        /// <summary>
        /// Permet de placer la plus petite image au milieu de la plus grande
        /// </summary>
        /// <param name="ImgToPlace"></param>
        /// <param name="StillImage"></param>
        public void PlaceSecondImage(MyImage ImgToPlace, MyImage StillImage) // S'assurer que Still Image est plus grand
        {
            int Xcenter = StillImage.PropLargeur / 2;
            int YCenter = StillImage.PropHauteur / 2;
            int OffsetX = Xcenter - (ImgToPlace.PropLargeur / 2);
            int OffsetY = YCenter - (ImgToPlace.PropHauteur / 2);
            int a = 0;
            int b = 0;
            for (int i = OffsetY; i < ImgToPlace.PropHauteur + OffsetY; i++)
            {
                for (int j = OffsetX; j < ImgToPlace.PropLargeur + OffsetX; j++)
                {
                    StillImage.PropImageRGB[i, j] = ImgToPlace.PropImageRGB[a, b];
                    b++;
                }
                a++;
                b = 0;
            }
        }

        /// <summary>
        /// Affecte des valeurs identiques a celle de notre objet courant à une matrice de pixels rentrés en paramètres.
        /// </summary>
        /// <param name="matrix"></param>
        public void SetEqualWithCurrent(Pixel[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = new Pixel(ImageRGB[i, j].Red, ImageRGB[i, j].Green, ImageRGB[i, j].Blue);
                }
            }
        }

        #endregion

        #region TD 3

        /// <summary>
        /// Applique un filtre avec un noyau de convolution en paramètre
        /// </summary>
        /// <param name="kernel"></param>
        public void ApplyFilter(int[,] kernel)
        {
            MyImage LargerImage = ImageForConvolution(kernel); // On agrandit l'image afin de pouvoir gétrer les bords.
            int offset = kernel.GetLength(0) / 2; // cela ne change rien car c'est une matrice carrée.
            for (int i = offset; i < LargerImage.PropImageRGB.GetLength(0) - offset; i++)
            {
                for (int j = offset; j < LargerImage.PropImageRGB.GetLength(1) - offset; j++)
                {
                    byte[,] RedMatrix = { { LargerImage.PropImageRGB[i - offset, j - offset].Red, LargerImage.PropImageRGB[i - offset, j - offset+1].Red, LargerImage.PropImageRGB[i - offset, j - offset + 2].Red },
                                          { LargerImage.PropImageRGB[i - offset + 1, j - offset].Red, LargerImage.PropImageRGB[i - offset + 1, j - offset + 1].Red, LargerImage.PropImageRGB[i - offset + 1, j - offset + 2].Red },
                                          { LargerImage.PropImageRGB[i - offset + 2, j - offset].Red, LargerImage.PropImageRGB[i - offset + 2, j - offset + 1].Red, LargerImage.PropImageRGB[i - offset + 2, j - offset + 2].Red }
                                        };
                    byte[,] GreenMatrix = { { LargerImage.PropImageRGB[i - offset, j - offset].Green, LargerImage.PropImageRGB[i - offset, j - offset+1].Green, LargerImage.PropImageRGB[i - offset, j - offset + 2].Green },
                                          { LargerImage.PropImageRGB[i - offset + 1, j - offset].Green, LargerImage.PropImageRGB[i - offset + 1, j - offset + 1].Green, LargerImage.PropImageRGB[i - offset + 1, j - offset + 2].Green },
                                          { LargerImage.PropImageRGB[i - offset + 2, j - offset].Green, LargerImage.PropImageRGB[i - offset + 2, j - offset + 1].Green, LargerImage.PropImageRGB[i - offset + 2, j - offset + 2].Green }
                                         };
                    byte[,] BlueMatrix = { { LargerImage.PropImageRGB[i - offset, j - offset].Blue, LargerImage.PropImageRGB[i - offset, j - offset+1].Blue, LargerImage.PropImageRGB[i - offset, j - offset + 2].Blue },
                                          { LargerImage.PropImageRGB[i - offset + 1, j - offset].Blue, LargerImage.PropImageRGB[i - offset + 1, j - offset + 1].Blue, LargerImage.PropImageRGB[i - offset + 1, j - offset + 2].Blue },
                                          { LargerImage.PropImageRGB[i - offset + 2, j - offset].Blue, LargerImage.PropImageRGB[i - offset + 2, j - offset + 1].Blue, LargerImage.PropImageRGB[i - offset + 2, j - offset + 2].Blue }
                                         }; // Ce sont les 3 matrices associés a chaque couleurs d'une zone 3x3.
                    byte SumRed = GetSum(kernel, RedMatrix); // On trouve la somme des coefficients de la multiplication du noyau avec les valeurs des pixels.
                    byte SumGreen = GetSum(kernel, GreenMatrix);
                    byte SumBlue = GetSum(kernel, BlueMatrix);
                    ImageRGB[i - offset, j - offset].Red = SumRed; // On affecte les valeurs au pixels.
                    ImageRGB[i - offset, j - offset].Green = SumGreen;
                    ImageRGB[i - offset, j - offset].Blue = SumBlue;
                }
            }
        }

        /// <summary>
        /// Retourne la somme des coefficients de la multiplication du noyau par les couleurs.
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public byte GetSum(int[,] kernel, byte[,] color)
        {
            int sum = 0;
            int SumValuesKernel = 0;
            for (int i = 0; i < color.GetLength(0); i++)
            {
                for (int j = 0; j < color.GetLength(1); j++)
                {
                    SumValuesKernel += kernel[i, j];
                    sum += kernel[i, j] * color[i, j];
                }
            }
            if (SumValuesKernel != 0 && SumValuesKernel != 1) { sum /= SumValuesKernel; } // Si la somme des coefficients du noyau est différente de 1 ou 0 alors on divise notre résultat par cette valeur.
            if (sum > 255) { sum = 255; }
            if (sum < 0 )  { sum = 0; }
            return (byte)sum;
        }

        /// <summary>
        /// On prépare l'image en prenant compte les bords pour la convolution.
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public MyImage ImageForConvolution(int[,] kernel)
        {
            MyImage res = new MyImage(2 * (kernel.GetLength(0) / 2) + hauteur, 2 * (kernel.GetLength(1) / 2) + largeur);
            for (int i = (kernel.GetLength(0) / 2); i < res.PropImageRGB.GetLength(0) - (kernel.GetLength(0) / 2); i++)
            {
                for (int j = (kernel.GetLength(1) / 2); j < res.PropImageRGB.GetLength(1) - (kernel.GetLength(1) / 2); j++) // On l'initilase avec les valeurs de l'image courante.
                {
                    res.PropImageRGB[i, j].Red = ImageRGB[i - (kernel.GetLength(0) / 2), j - (kernel.GetLength(1) / 2)].Red;
                    res.PropImageRGB[i, j].Green = ImageRGB[i - (kernel.GetLength(0) / 2), j - (kernel.GetLength(1) / 2)].Green;
                    res.PropImageRGB[i, j].Blue = ImageRGB[i - (kernel.GetLength(0) / 2), j - (kernel.GetLength(1) / 2)].Blue;
                }
            }
            
            for (int i = 0; i < res.PropImageRGB.GetLength(0); i++) // En fonction de où on se trouve dans l'image on va prendre les pixels adjacents d'où la différenciation des cas.
            {
                if (i < (kernel.GetLength(0) / 2)) //Si nous sommes avant la première ligne
                {
                    for (int j = 0; j < res.PropImageRGB.GetLength(1); j++)
                    {
                        if (j <= (kernel.GetLength(0) / 2)) //Si nous sommes à la première colonne
                        {
                            res.PropImageRGB[i, j] = ImageRGB[0, 0];
                        }
                        else if (j > (kernel.GetLength(0) / 2) && j < res.PropImageRGB.GetLength(1) - 1 - (kernel.GetLength(0) / 2)) // Si nous sommes entre la deuxième et avant-dernière colonne
                        {
                            res.PropImageRGB[i, j] = ImageRGB[0, j - (kernel.GetLength(1) / 2)];
                        }
                        else if (j >= res.PropImageRGB.GetLength(1) - 1 - (kernel.GetLength(0) / 2))// Si nous sommes à la dernière colonne
                        {
                            res.PropImageRGB[i, j] = ImageRGB[0, ImageRGB.GetLength(1) - 1];
                        }
                    }
                }
                else if (i >= ImageRGB.GetLength(0) + (kernel.GetLength(0) / 2)) // Si nous sommes à la dernière ligne
                {
                    for (int j = 0; j < res.PropImageRGB.GetLength(1); j++)
                    {
                        if (j <= (kernel.GetLength(0) / 2))//Si nous sommes à la première colonne
                        {
                            res.PropImageRGB[i, j] = ImageRGB[ImageRGB.GetLength(0) - 1, 0];
                        }
                        else if (j > (kernel.GetLength(0) / 2) && j < res.PropImageRGB.GetLength(1) - 1 - (kernel.GetLength(0) / 2)) // Si nous sommes entre la deuxième et avant-dernière colonne
                        {
                            res.PropImageRGB[i, j] = ImageRGB[ImageRGB.GetLength(0) - 1, j - (kernel.GetLength(1) / 2)];
                        }
                        else if (j >= res.PropImageRGB.GetLength(1) - 1 - (kernel.GetLength(0) / 2))// Si nous sommes à la dernière colonne
                        {
                            res.PropImageRGB[i, j]= ImageRGB[ImageRGB.GetLength(0) - 1, ImageRGB.GetLength(1) - 1];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < (kernel.GetLength(1) / 2); j++) // On traite tout les autres pixels
                    {
                        res.PropImageRGB[i, j] = ImageRGB[i - (kernel.GetLength(0) / 2), 0];
                    }
                    for (int j = (kernel.GetLength(0) / 2) + ImageRGB.GetLength(1); j < res.PropImageRGB.GetLength(1); j++)
                    {
                        res.PropImageRGB[i, j] = ImageRGB[i - (kernel.GetLength(0) / 2), ImageRGB.GetLength(1) - 1];
                    }
                }
            }
            return res;
        }

        #endregion

        #region TD 4

        /// <summary>
        /// Cette méthode dessine un carré noir au centre d'une image 
        /// </summary>
        public void FormeCarree()
        {
            int centerX = (int)Math.Round((double)largeur / 2);
            int centerY = (int)Math.Round((double)hauteur / 2); // On arrondi a l'entier supérieur pour éviter de sortiR des limites pour les images non multiples de 2.
            for (int i = centerY / 2; i < hauteur - centerY/2; i++)
            {
                for (int j = centerX - centerY/2; j < largeur - (centerX - centerY / 2); j++)
                {
                    ImageRGB[i, j].Red = 0;
                    ImageRGB[i, j].Green = 0;
                    ImageRGB[i, j].Blue = 0;
                }
            }
        }

        /// <summary>
        /// Cette éthode dessine un rectangle au centre d'une image
        /// </summary>
        public void FormeTriangle()
        {
            int centerX = (int)Math.Round((double)largeur / 2);
            int centerY = (int)Math.Round((double)hauteur / 2); // On arrondi a l'entier supérieur pour éviter de sortiR des limites pour les images non multiples de 2.
            int offset = 0;
            for (int i = centerY/3; i < hauteur - centerY/3; i++)
            {
                for (int j = centerX - (hauteur - 2*centerY/3)/2 + offset; j < largeur - (centerX - (hauteur - 2*centerY/3) / 2 + offset); j++)
                {
                    ImageRGB[i, j].Red = 0;
                    ImageRGB[i, j].Green = 0;
                    ImageRGB[i, j].Blue = 0;
                }
                if (i % 2 == 0) { offset++; }
            }
        }

        /// <summary>
        /// Cette méthode permet de créer les 3 histogrammes associés à une image (ou 1 si elle est en nuances de gris)
        /// </summary>
        public void ColorsToHistograms()
        {
            int[] Rouge = new int[256];
            int[] Vert = new int[256];
            int[] Bleu = new int[256];
            int[] Monoscale = new int[256];
            for (int i = 0; i < Rouge.Length; i++) // On initialise nos tableaux de couleurs à 0
            {
                Rouge[i] = 0;
                Vert[i] = 0;
                Bleu[i] = 0;
                Monoscale[i] = 0;
            }
            int valeur = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++) // On parcourt toute la matrice de pixels et on incrémente la valeur de la nuance correspondante dans le tableau à chaque fois qu'on la trouve.
                {
                    valeur = ImageRGB[i, j].Blue;
                    Bleu[valeur]++;
                    valeur = ImageRGB[i, j].Green;
                    Vert[valeur]++;
                    valeur = ImageRGB[i, j].Red;
                    Rouge[valeur]++;
                    Monoscale[valeur]++;
                }
            }           
            if (IsAllGrey) 
                CreateHistogram(Monoscale, "monoscale"); 
            else
            {
                CreateHistogram(Rouge, "rouge"); 
                CreateHistogram(Vert, "vert");
                CreateHistogram(Bleu, "bleu");
            }           
        }

        /// <summary>
        /// Cette méthode retourne le maximum d'un tableau
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int GetMax(int[] tab)
        {
            int max = 0;
            for (int i = 0; i < tab.Length; i++)
            { 
                if(tab[i]>max) { max = tab[i]; }
            }
            return max;
        }

        /// <summary>
        /// Cette méthode crée des images correspondantes aux histogrammes en fonction de la couleur
        /// </summary>
        /// <param name="IntensityForColors"></param>
        /// <param name="color"></param>
        public void CreateHistogram(int[] IntensityForColors, string color)
        {
            MyImage Histo = new MyImage(100, 256);  
            int Max = GetMax(IntensityForColors); // On cherche la nuance la plus présente
            for (int i = 0; i < IntensityForColors.Length; i++)
            {
                IntensityForColors[i] = (int)((double)IntensityForColors[i] / Max * 100); // On exprime les valeurs en pourcent
            }

            for (int i = 0; i < Histo.PropImageRGB.GetLength(1); i++)
            {
                int j = 0;
                while (j < IntensityForColors[i]) // On trace la couleur en fonction de son intensité sur l'image.=
                {
                    if (color == "rouge")
                    {
                        Histo.PropImageRGB[j, i].Red = 255;
                        Histo.PropImageRGB[j, i].Green = 0;
                        Histo.PropImageRGB[j, i].Blue = 0;
                    }
                    else if (color == "vert")
                    {
                        Histo.PropImageRGB[j, i].Red = 0;
                        Histo.PropImageRGB[j, i].Green = 255;
                        Histo.PropImageRGB[j, i].Blue = 0;
                    }
                    else if (color == "bleu")
                    {
                        Histo.PropImageRGB[j, i].Blue = 255;
                        Histo.PropImageRGB[j, i].Green = 0;
                        Histo.PropImageRGB[j, i].Red = 0;
                    }
                    else if (color == "monoscale")
                    {
                        Histo.PropImageRGB[j, i].Red = 0;
                        Histo.PropImageRGB[j, i].Green = 0;
                        Histo.PropImageRGB[j, i].Blue = 0;
                    }
                    j++;
                }            
            }
            Histo.From_Image_To_File("Histogramme "+color+".bmp");
            Process.Start("Histogramme "+color+".bmp");
        }

        #endregion

        #region TD 5

        /// <summary>
        /// Cette méthode permet d'appliquer une corrrection gamma en fonction de facteurs pour chauqe couleurs. (0.2 à 5)
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <returns></returns>
        public MyImage Gamma(double red, double green, double blue)
        {
            MyImage temp = new MyImage(hauteur, largeur);
            SetEqualWithCurrent(temp.ImageRGB); 
            byte[] RedGamma = CreateGamma(red);
            byte[] GreenGamma = CreateGamma(green);
            byte[] BlueGamma = CreateGamma(blue); // On crée les corrections gamma pour chaque couleur.
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    temp.ImageRGB[i, j].Red = BlueGamma[ImageRGB[i, j].Blue];
                    temp.ImageRGB[i, j].Green = GreenGamma[ImageRGB[i, j].Green]; 
                    temp.ImageRGB[i, j].Blue = RedGamma[ImageRGB[i, j].Red];
                }
            }
            return temp;
        } 

        /// <summary>
        /// Cette méthode retourne le tableau de correction gamma en fonction de la couleur.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public byte[] CreateGamma(double color)
       {
            byte[] gammatab = new byte[256];
            for (int i = 0; i < 256; ++i)
            {
                gammatab[i] = (byte)Math.Min(255,(int)((255 * Math.Pow(i / 255, 1 / color)) + 0.5)); // Formule de la correction gamma.
            }
            return gammatab;
       }

        /// <summary>
        /// Cette méthode modifie la luminosité de l'image (-255 à 255)
        /// </summary>
        /// <param name="brightness"></param>
        public void SetBrightness(int brightness)
        {
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    int RedColor = ImageRGB[i, j].Red + brightness; // On ajoute le facteur à chaque pixel
                    int GreenColor = ImageRGB[i, j].Green + brightness;
                    int BlueColor = ImageRGB[i, j].Blue + brightness;

                    if (RedColor < 0) RedColor = 1;
                    if (RedColor > 255) RedColor = 255;

                    if (GreenColor < 0) GreenColor = 1;
                    if (GreenColor > 255) GreenColor = 255;

                    if (BlueColor < 0) BlueColor = 1;
                    if (BlueColor > 255) BlueColor = 255; // On les normalize en format de bytes

                    ImageRGB[i, j].Red = (byte)RedColor;
                    ImageRGB[i, j].Green = (byte)GreenColor;
                    ImageRGB[i, j].Blue = (byte)BlueColor;
                }
            }
        } 

        /// <summary>
        /// Cette méthode modifie le contraste d'une image (-100 à 100)
        /// </summary>
        /// <param name="contrast"></param>
        public void SetContrast(double contrast)
        {
            contrast = (100 + contrast) / 100.0; // On convertit notre facteur entre 0 et 2 
            contrast *= contrast; // On élève au carré
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    double percentRed = ImageRGB[i,j].Red / 255.0; // Convertit la nuance en pourcentage
                    percentRed -= 0.5;
                    percentRed *= contrast;
                    percentRed += 0.5;
                    percentRed *= 255; // Manipulation pour changer le contraste
                    if (percentRed < 0) percentRed = 0;
                    if (percentRed > 255) percentRed = 255; // Normalisation en byte

                    double percentGreen = ImageRGB[i, j].Green / 255.0;
                    percentGreen -= 0.5;
                    percentGreen *= contrast;
                    percentGreen += 0.5;
                    percentGreen *= 255;
                    if (percentGreen < 0) percentGreen = 0;
                    if (percentGreen > 255) percentGreen = 255;

                    double percentBlue = ImageRGB[i, j].Blue / 255.0;
                    percentBlue -= 0.5;
                    percentBlue *= contrast;
                    percentBlue += 0.5;
                    percentBlue *= 255;
                    if (percentBlue < 0) percentBlue = 0;
                    if (percentBlue > 255) percentBlue = 255;

                    ImageRGB[i, j].Red = (byte)percentRed;
                    ImageRGB[i, j].Green = (byte)percentGreen;
                    ImageRGB[i, j].Blue = (byte)percentBlue;
                }
            }
            
        }

        /// <summary>
        /// Actualiser les valeurs RGB avec les valeurs HSV
        /// </summary>
        /// <param name="P"></param>
        public void UpdateRGBWithHSV(Pixel P)
        {
            double C = P.Saturation * P.Value; 
            double X = C * (1 - Math.Abs(((P.Hue / 60) % 2) - 1));
            double m = P.Value - C;
            if (P.Hue != 360) { P.Hue /= 60; }
            else { P.Hue = 0; }
            int Compare = (int)Math.Truncate(P.Hue);
            double Red = 0, Green = 0, Blue = 0;
            switch(Compare)
            {
                case 0:
                    Red = C;
                    Green = X;
                    break;
                case 1:
                    Red = X;
                    Green = C;
                    break;
                case 2:
                    Green = C;
                    Blue = X;
                    break;
                case 3:
                    Green = X;
                    Blue = C;
                    break;
                case 4:
                    Red = X;
                    Blue = C;
                    break;
                default :
                    Red = C;
                    Blue = X;
                    break;
            }
            P.Red = (byte)((Red + m)*255);
            P.Green = (byte)((Green + m) * 255);
            P.Blue = (byte)((Blue + m) * 255);
        }

        /// <summary>
        /// Actualiser les valeurs HSV avec les valeurs RGB
        /// </summary>
        /// <param name="P"></param>
        public void UpdateHSVWithRGB(Pixel P)
        {
            P.Hue = 0;
            double min = Math.Min(Math.Min(P.Red, P.Green), P.Blue);
            P.Value = Math.Max(Math.Max(P.Red, P.Green), P.Blue);
            double Delta = P.Value - min;

            if (P.Value == 0) { P.Saturation = 0; }
            else { P.Saturation = Delta / P.Value; }

            if (P.Saturation != 0)
            {
                if (P.Red == P.Value) { P.Hue = ((P.Green - P.Blue) / Delta) % 6; }

                else if (P.Green == P.Value) { P.Hue = 2 + (P.Blue - P.Red) / Delta; }

                else if (P.Blue == P.Value) { P.Hue = 4 + (P.Red - P.Green) / Delta; }

                P.Hue *= 60;

                if (P.Hue < 0) { P.Hue += 360; }
            }
            P.Value /= 255;
        }

        #endregion

        #region HDR

        /// <summary>
        /// Retourne une image caractéristique de la bonne exposition de l'image
        /// </summary>
        /// <returns></returns>
        public MyImage GetWellExposedness()
        {
            MyImage res = new MyImage(hauteur, largeur);
            SetEqualWithCurrent(res.ImageRGB);
            for (int i = 0; i < res.hauteur; i++)
            {
                for (int j = 0; j < res.largeur; j++)
                {
                    double ExposureFactor = Math.Exp(-(Math.Pow(res.ImageRGB[i,j].Value - 0.5,2) / 0.08)); // Formalisation de l'exposition d'un pixel
                    res.ImageRGB[i, j].Value = ExposureFactor;
                }
            }
            return res;
        }

        /// <summary>
        /// Augmente ou diminue l'importance de l'exposition dans l'image
        /// </summary>
        /// <param name="factorWE"></param>
        /// <returns></returns>
        public MyImage GetOverallWeightMap(double factorWE)
        {
            MyImage W = GetWellExposedness();
            MyImage res = new MyImage(hauteur, largeur);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    res.ImageRGB[i, j].Value = Math.Pow(W.ImageRGB[i, j].Value, factorWE); // On élève le facteur à la puissance souhaitée
                    UpdateRGBWithHSV(res.ImageRGB[i, j]);
                    res.ImageRGB[i, j].Value += Math.Pow(10, -12); // On évite 0
                }
            }
            return res;
        }

        /// <summary>
        /// Normalise les images caractéristiques de la bonne exposition pour chaque image de la série HDR
        /// </summary>
        /// <param name="Weights"></param>
        public void NormalizeWeight(MyImage[] Weights)
        {
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    double Sum = 0;
                    for (int k = 0; k < Weights.Length; k++) // On parcours les images caractéristiques 
                    {
                        Sum += Weights[k].ImageRGB[i, j].Value;
                    }
                    ImageRGB[i, j].Value /= Sum; // On normalize les valeurs en les divisant pas la somme des coefficients
                }
            }
        }

        /// <summary>
        /// Effectue la fusion d'exposition effet HDR
        /// </summary>
        /// <param name="Weights"></param>
        /// <param name="Images"></param>
        /// <returns></returns>
        public MyImage NaiveBlending(MyImage[] Weights, MyImage[] Images)
        {
            MyImage res = new MyImage(hauteur, largeur);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    for (int k = 0; k < Weights.Length; k++) // On parcourt chaque pixel pour chaque image caractéristique
                    {
                        res.ImageRGB[i, j].Red += (byte)(Images[k].ImageRGB[i, j].Red * Weights[k].ImageRGB[i, j].Value); 
                        res.ImageRGB[i, j].Green += (byte)(Images[k].ImageRGB[i, j].Green * Weights[k].ImageRGB[i, j].Value);
                        res.ImageRGB[i, j].Blue += (byte)(Images[k].ImageRGB[i, j].Blue * Weights[k].ImageRGB[i, j].Value); // Les pixels de l'image finale sont le résultats d'une multiplication de la valeur du pixel de l'iamge caractéristique avec la valeur associée à ce pixel dans l'image d'origine.
                        UpdateHSVWithRGB(res.ImageRGB[i, j]);
                        res.ImageRGB[i, j].Value *= 1.07; // On augmente un peu l'exposition pour accentuer le résultat.
                        UpdateRGBWithHSV(res.ImageRGB[i, j]);
                    }
                }
            }
            return res;
        }

        #endregion

        #region Double Exposition

        /// <summary>
        /// Retourne un masque noir des contours de l'image à remplir
        /// </summary>
        /// <returns></returns>
        public MyImage GetMask()
        {
            MyImage res = new MyImage(hauteur, largeur);
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (ImageRGB[i,j].Value != 1) // Si le pixel n'est pas blanc
                    {
                        res.ImageRGB[i, j].Value = 0;
                        UpdateRGBWithHSV(res.ImageRGB[i, j]);
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Retourne des infos sur le placement du masque
        /// </summary>
        /// <returns></returns>
        public int[] Infos()
        {
            int[] Infos = new int[4];
            int StartRow = 0;
            int StartColumn = 0;
            int EndRow = 0;
            int EndColumn = 0;
            for (int i = 0; i < hauteur; i++)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if(ImageRGB[i,j].Value == 0) { StartRow = i; break; } // On cherche la ligne ou le masque débute.
                }
                if (StartRow != 0) break;
            }
            for (int i = hauteur-1; i >= 0; i--)
            {
                for (int j = 0; j < largeur; j++)
                {
                    if (ImageRGB[i, j].Value == 0) { EndRow = i; break; } // On cherche la ligne ou le masque finit.
                }
                if (EndRow != 0) break;
            }
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    if (ImageRGB[j, i].Value == 0) { StartColumn = i; break; } // On cherche la colonne ou le masque débute.
                }
                if (StartColumn != 0) break;
            }
            for (int i = largeur-1; i >= 0; i--)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    if (ImageRGB[j, i].Value == 0) { EndColumn = i; break; }// On cherche la colonne ou le masque finit.
                }
                if (EndColumn != 0) break;
            }
            Infos[0] = StartRow;
            Infos[1] = EndRow;
            Infos[2] = StartColumn;
            Infos[3] = EndColumn;
            return Infos;
        }

        /// <summary>
        /// On remplit l'image avec un dégradé linéaire entre l'image de fond et le masque, en noir et blanc ou couleur, en fonction d'une direction
        /// </summary>
        /// <param name="Image1"></param>
        /// <param name="Image2"></param>
        /// <param name="Infos"></param>
        /// <param name="how"></param>
        /// <param name="ToGrey"></param>
        public void Fill(MyImage Image1, MyImage Image2, int[] Infos, string how, bool ToGrey)
        {
            int DistanceHeight = Infos[1] - Infos[0];
            int DistanceWidth = Infos[3] - Infos[2];
            Image1.SetContrast(-10);
            Image2.SetContrast(10);
            Image1.SetBrightness(-10);
            Image2.SetBrightness(10); // Modification de contraste et luminosité
            if (ToGrey) Image1.ColorToGreys();
            if (how == "BottomToTop") // Le remplissage change en fonction de la direction dans lequel on veut l'appliquer
            {
                for (int i = Infos[0]; i <= Infos[1]; i++)
                {
                    for (int j = Infos[2]; j <= Infos[3]; j++)
                    {
                        if(ImageRGB[i,j].Value == 0)
                        {
                            ImageRGB[i, j].Red = (byte)((((double)(Infos[1] - i) / DistanceHeight) * Image1.ImageRGB[i, j].Red) + (((double)(i - Infos[0]) / DistanceHeight) * Image2.ImageRGB[i, j].Red)); //Formule pour remplir de haut en bas
                            ImageRGB[i, j].Green = (byte)((((double)(Infos[1] - i) / DistanceHeight) * Image1.ImageRGB[i, j].Green) + (((double)(i - Infos[0]) / DistanceHeight) * Image2.ImageRGB[i, j].Green));
                            ImageRGB[i, j].Blue = (byte)((((double)(Infos[1] - i) / DistanceHeight) * Image1.ImageRGB[i, j].Blue) + (((double)(i - Infos[0]) / DistanceHeight) * Image2.ImageRGB[i, j].Blue));
                        }
                    }
                }
            }
            if (how == "TopToBottom")
            {
                for (int i = Infos[0]; i <= Infos[1]; i++)
                {
                    for (int j = Infos[2]; j <= Infos[3]; j++)
                    {
                        if (ImageRGB[i, j].Value == 0)
                        {
                            ImageRGB[i, j].Red = (byte)((((double)(i - Infos[0]) / DistanceHeight) * Image1.ImageRGB[i, j].Red) + (((double)(Infos[1] - i) / DistanceHeight) * Image2.ImageRGB[i, j].Red));  //Formule pour remplir de bas en haut
                            ImageRGB[i, j].Green = (byte)((((double)(i - Infos[0]) / DistanceHeight) * Image1.ImageRGB[i, j].Green) + (((double)(Infos[1] - i) / DistanceHeight) * Image2.ImageRGB[i, j].Green));
                            ImageRGB[i, j].Blue = (byte)((((double)(i - Infos[0]) / DistanceHeight) * Image1.ImageRGB[i, j].Blue) + (((double)(Infos[1] - i) / DistanceHeight) * Image2.ImageRGB[i, j].Blue));
                        }
                    }
                }
            }
            if (how == "LeftToRight") 
            {
                for (int i = Infos[2]; i <= Infos[3]; i++)
                {
                    for (int j = Infos[0]; j <= Infos[1]; j++)
                    {
                        if (ImageRGB[j, i].Value == 0)
                        {
                            ImageRGB[j, i].Red = (byte)((((double)(i - Infos[2]) / DistanceWidth) * Image1.ImageRGB[j, i].Red) + (((double)(Infos[3] - i) / DistanceWidth) * Image2.ImageRGB[j, i].Red)); //Formule pour remplir de gauche à droite 
                            ImageRGB[j, i].Green = (byte)((((double)(i - Infos[2]) / DistanceWidth) * Image1.ImageRGB[j, i].Green) + (((double)(Infos[3] - i) / DistanceWidth) * Image2.ImageRGB[j, i].Green));
                            ImageRGB[j, i].Blue = (byte)((((double)(i - Infos[2]) / DistanceWidth) * Image1.ImageRGB[j, i].Blue) + (((double)(Infos[3] - i) / DistanceWidth) * Image2.ImageRGB[j, i].Blue));
                        }
                    }
                }
            }
            if (how == "RightToLeft")
            {
                for (int i = Infos[2]; i <= Infos[3]; i++)
                {
                    for (int j = Infos[0]; j <= Infos[1]; j++)
                    {
                        if (ImageRGB[j, i].Value == 0)
                        {
                            ImageRGB[j, i].Red = (byte)((((double)(Infos[3] - i) / DistanceWidth) * Image1.ImageRGB[j, i].Red) + (((double)(i - Infos[2]) / DistanceWidth) * Image2.ImageRGB[j, i].Red));  //Formule pour remplir de droite à gauche
                            ImageRGB[j, i].Green = (byte)((((double)(Infos[3] - i) / DistanceWidth) * Image1.ImageRGB[j, i].Green) + (((double)(i - Infos[2]) / DistanceWidth) * Image2.ImageRGB[j, i].Green));
                            ImageRGB[j, i].Blue = (byte)((((double)(Infos[3] - i) / DistanceWidth) * Image1.ImageRGB[j, i].Blue) + (((double)(i - Infos[2]) / DistanceWidth) * Image2.ImageRGB[j, i].Blue));
                        }
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Crée une réflection géométrique en fonction de la forme voulue
        /// </summary>
        /// <param name="shape"></param>
        public void GeometricReflection(string shape)
        {
            MyImage mask = new MyImage(hauteur, largeur);
            MyImage Notblurred = new MyImage(hauteur, largeur); // On crée une image non floutée égale à notre objet courant
            SetEqualWithCurrent(Notblurred.ImageRGB);
            Notblurred.Rotation(180);
            int[,] blur = { { 1, 1, 1 }, { 1, 1, 1}, { 1, 1, 1 } };
            ApplyFilter(blur);
            SetContrast(-20);
            SetBrightness(-25); // On rend flou notre image courante
            if (shape == "carré") mask.FormeCarree();
            else if (shape == "triangle") mask.FormeTriangle();
            else if (shape == "triangle inversé")
            {
                mask.FormeTriangle();
                mask.Rotation(180);
            } // on dessinne la forme voulue sur le masque
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {                   
                    if (mask.ImageRGB[i,j].Red == 0) // Si nous sommes sur le masque alors on place les pixels de l'image d'origine
                    {
                        ImageRGB[i, j] = Notblurred.ImageRGB[i, j];
                    }
                }
            }
        }

        #region Utilities

        /// <summary>
        /// Affiche la matrice de pixels
        /// </summary>
        public void AffichageMatriceRGB()
        {
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    Console.Write(ImageRGB[i, j].Red + " ");
                    Console.Write(ImageRGB[i, j].Green + " ");
                    Console.Write(ImageRGB[i, j].Blue + " | ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Rempli le constructeur en fonction d'un index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] FillConstructor(int index)
        {
            byte[] Tab = new byte[4];
            int j = index;
            while (j < index+4)
            {
                for (int i = 0; i < 4; i++)
                {
                    Tab[i] = tabDeBytes[j];
                    j++;
                }
            }
            return Tab;
        }
        
        /// <summary>
        /// Affiche la matrice HSV
        /// </summary>
        public void AffichageMatriceHSV()
        {
            for (int i = 0; i < ImageRGB.GetLength(0); i++)
            {
                for (int j = 0; j < ImageRGB.GetLength(1); j++)
                {
                    Console.Write(ImageRGB[i, j].Hue + " ");
                    Console.Write(ImageRGB[i, j].Saturation + " ");
                    Console.Write(ImageRGB[i, j].Value + " | ");
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Permet d'obtenir les bytes du fichier .csv
        /// </summary>
        /// <param name="fichier"></param>
        /// <returns></returns>
        public List<int> BytesDuFichier(string fichier)
        {
            List<int> EntierDuFichier = new List<int>();
            string lines = File.ReadAllText(fichier);
            char[] delimiters = new char[] { ';', '\r', '\n' };
            string[] splitted = lines.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitted.Length; i++)
            {
               EntierDuFichier.Add(int.Parse(splitted[i])); 
            }
            return EntierDuFichier;
        }

        /// <summary>
        /// Retourne le PGCD de manière récursive 
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static int PGCD(int val1, int val2)
        {
            int res = 0;
            if (val2 == 0) { res = val1; }
            else { res = PGCD(val2, val1 % val2); }
            return res;
        }

        /// <summary>
        /// Retourne le max de deux valeurs
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        /// <returns></returns>
        public static int Max(int val1, int val2)
        {
            int res = val1;
            if(val2>val1) { res = val2; }
            return res;
        }

        /// <summary>
        /// Affiche les bytes d'une image
        /// </summary>
        public void AffichageInfos()
        {
            Console.WriteLine("Header \n");
            for (int i = 0; i < 14; i++)
                Console.Write(tabDeBytes[i] + " ");
            Console.WriteLine("\nHEADER INFO \n\n");
            for (int i = 14; i < 54; i++)
                Console.Write(tabDeBytes[i] + " ");
            Console.WriteLine("\n\nIMAGE \n");
            for (int i = 54; i < tabDeBytes.Length; i++)
            {
                Console.Write(tabDeBytes[i]+"\t");
            }
        }

        /// <summary>
        /// Applique une correction assez générique sur une image
        /// </summary>
        public void PostProcessing()
        {
            SetBrightness(20);
            SetContrast(20);
            Gamma(2.4, 2.4, 2.4); // valeur courante sur l'affichage de moniteurs
        }

        #endregion

    }
}
