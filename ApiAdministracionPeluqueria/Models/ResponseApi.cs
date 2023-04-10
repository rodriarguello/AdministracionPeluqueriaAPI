namespace ApiAdministracionPeluqueria.Models
{
    public class ResponseApi
    {
        


        public ModeloRespuesta respuestaExitosa( object data = null)
        {


            var respuesta = new ModeloRespuesta(1, null, data);

            return respuesta;

        }

        public ModeloRespuesta respuestaError(string mensaje) { 
        
            
            var respuesta = new ModeloRespuesta(0,mensaje,null);

            return respuesta;

        }
    }
}
