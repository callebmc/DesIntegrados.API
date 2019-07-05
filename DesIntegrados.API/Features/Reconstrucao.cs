using DotNumerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace DesIntegrados.API.Features
{
    public class Reconstrucao
    {
        static IList<float> Ultrasound;
        static float[] ultravector;
        static Matrix H = null, g, x;
        static string tamanho;
        static float ganho;
        static string arquivoG;
        public static void IniciaReconstrucao(float gain, string g)
        {
            ganho = gain;
            arquivoG = g;
            Console.WriteLine("OI");
            H = new Matrix(50816, 3600);
            string path = @"C:\Users\Calleb Malinoski\Desktop\H-1.txt";
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            for (int i = 0; i < 50816; i++)
            {
                string[] bufferA = file.ReadLine().Split(',');
                for (int j = 0; j < 3600; j++)
                    if (!String.IsNullOrWhiteSpace(bufferA[j]))
                        H[i, j] = float.Parse(bufferA[j], CultureInfo.InvariantCulture);
            }
            file.Close();

            Ultrasound = new List<float>();
            path = @"C:\Users\Calleb Malinoski\Desktop\"+g+".txt";
            string[] buffer = File.ReadAllText(path).Split('\n');
            Ultrasound = new List<float>();
            foreach (string number in buffer)
            {
                if (!String.IsNullOrWhiteSpace(number))
                    Ultrasound.Add(float.Parse(number, CultureInfo.InvariantCulture));
            }
            Console.WriteLine(Ultrasound.Count.ToString());
            Console.WriteLine("Carreguei");
            ultravector = AjustUltraSoundGain(gain);
            CriaImagem();
        }
        static float[] AjustUltraSoundGain(float gain)
        {
            if (gain != 1)
            {
                int i = 0;
                int size = Ultrasound.Count;
                int tenPer = size / 10;
                float auxG;
                float[] ret = new float[size];
                auxG = (gain / 2 - gain / 20);
                foreach (float value in Ultrasound)
                {
                    if (i % tenPer == 0)
                        auxG += gain / 20;
                    ret[i] = value * auxG;
                    i++;
                }
                return ret;
            }
            return Ultrasound.ToArray();
        }
        //serve pra multiplicar com transposta sem precisar alocar nova matrix na memória, por que da out of memory exception
        static Matrix MatrixMultTranpose(Matrix a, Matrix b)
        {
            int col = b.ColumnCount, row = a.ColumnCount;
            int m = a.RowCount;
            Matrix c = new Matrix(row, col);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m; k++)
                        sum += a[k, i] * b[k, j];
                    c[i, j] = sum;
                }
            }
            return c;
        }
        /*H é a matrix gigante de tramanho(50816, 3600), 
         * g é o vetor de ultrasom passado de tamanho (50816, 1)
         * f é a saida de tamanho (3600,1) que vai ser iniciada em 0
         * O tamanho 50816 vem do tamannho do vetor de entrada
         * O tamanho 3600 vem do tamanho da imagem final (60X60)
         */
        static void CGNE(Matrix H, Matrix g, out Matrix f)
        {
            f = new Matrix(3600, 1);
            for (int i = 0; i < 3600; i++)//f0=0
            {
                f[i, 0] = 0;
            }
            Matrix r = g - H * f;//r0 = g - Hf0

            Matrix p = MatrixMultTranpose(H, r); //p0 = HTr0
            double a, B;
            double rtXr = (r.Transpose() * r)[0, 0]; //=riT * ri serve pra não precisar calcular duas vezes
            double ritXri;//=ri+1T * ri+1 serve pra não precisar calcular duas vezes
            Matrix ri;// ri+1
            for (int i = 0; i < 15; i++)
            {
                a = rtXr / (p.Transpose() * p)[0, 0];//ai = riT * ri / piT * pi
                f = f + p.Multiply(a);//fi+1 = fi + ai * pi
                ri = r - (H * p).Multiply(a);//ri+1 = ri - ai * H * pi
                ritXri = (ri.Transpose() * ri)[0, 0];//=ri+1T * ri+1
                B = ritXri / rtXr;//Bi = ri+1T * ri+1 / riT * ri
                p = MatrixMultTranpose(H, ri) + p.Multiply(B);//pi = HT * ri+1 + Bi * pi
                r = ri;// ri = ri+1
                rtXr = ritXri;
            }
        }

        static async void CriaImagem()
        {
            g = new Matrix(50816, 1);
            for (int i = 0; i < 50816; i++)//constroi matrix do vetor de entrada
            {
                g[i, 0] = ultravector[i];
            }

            CGNE(H, g, out x);

            //Parte que atribui valor de 0 até 255 para a imagem
            Bitmap bmp = new Bitmap(60, 60);
            double max = double.NegativeInfinity, min = double.PositiveInfinity;
            for (int i = 0; i < 3600; i++)//Calcula valor máximo e mínimo da imagem
            {
                if (x[i, 0] > max)
                    max = x[i, 0];
                if (x[i, 0] < min)
                    min = x[i, 0];
            }
            int k = 0;
            int value;
            for (int i = 0; i < 60; i++)
                for (int j = 0; j < 60; j++)
                {
                    value = (int)((255 / (max - min)) * (x[k, 0] - min));
                    bmp.SetPixel(i, j, Color.FromArgb(value, value, value));
                    k++;
                }

            //string storageConnection = CloudConfigurationManager.GetSetting("TkgOnGxOzNj+AS//xEMoXTQUZIFCsjp/C54DnkjWwddJpDuubv56ve0/pWgyx+dAtafX8m8RzAhGql4ZlOiAeQ==")
            //CloudStorageAccount cloud
            bmp.Save(@"C:\Users\Calleb Malinoski\Desktop\img2.bmp");
            ///////////*var storageCredentials = new StorageCredentials("desintegrados", "TkgOnGxOzNj+AS//xEMoXTQUZIFCsjp/C54DnkjWwddJpDuubv56ve0/pWgyx+dAtafX8m8RzAhGql4ZlOiAeQ==");
            //////////var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            //////////var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            ////////////Create Reference to Azure Blob
            ////////////CloudBlobClient blobClient = storageacc.CreateCloudBlobClient();

            ////////////The next 2 lines create if not exists a container named "democontainer"
            //////////CloudBlobContainer container = blobClient.GetContainerReference("desintegradosimages");
            ////////////await container.CreateIfNotExistsAsync();

            ////////////container.CreateIfNotExists();

            ////////////The next 7 lines upload the file test.txt with the name DemoBlob on the container "democontainer"
            //////////CloudBlockBlob blockBlob = container.GetBlockBlobReference("desintegradosimages.bmp");
            //////////await blockBlob.UploadFromFileAsync(@"C:\Users\Calleb Malinoski\Desktop\img2.bmp");

            ///////////*https://dotnetcoretutorials.com/2017/06/17/using-azure-blob-storage-net-core/-*/*/
        }
    }
}
