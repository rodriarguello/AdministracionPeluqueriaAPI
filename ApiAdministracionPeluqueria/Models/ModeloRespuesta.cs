namespace ApiAdministracionPeluqueria.Models
{
    public class ModeloRespuesta
    {
        public ModeloRespuesta(int resultado, string mensaje, object data)
        {
            Resultado = resultado;
            Mensaje = mensaje;
            Data = data;
        

        }


        public int Resultado; 

        public string Mensaje;

        public  object Data; 
    }
}
