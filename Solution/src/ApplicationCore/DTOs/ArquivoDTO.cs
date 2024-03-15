namespace Syngenta.Casa.RenCad.AppHub.ApplicationCore.DTOs
{
    public class ArquivoDTO
        {
            public int Id
            {
                get; set;
            }
            public string Descricao
            {
                get; set;
            }
            public string Nome
            {
                get; set;
            }
            public string Tipo
            {
                get; set;
            }
            public string Cpf_cnpj
            {
                get; set;
            }

            public string Base64
            {
                get; set;
            }public byte[] Base64Bytes
            {
                get; set;
            }
            public string Extensao
            {
                get; set;
            }
            public string Comentario
            {
                get; set;
            }
            public int Ano
            {
                get; set;
            }

            public string UsuarioEnviou
            {
                get; set;
            }
            
            public string UsuarioRevisou
            {
                get; set;
            }
            public string DataEnvio
            {
                get; set;
            }
            
            public string DataRevisao
            {
                get; set;
            }
            
            public string URL
            {
                get; set;
            }

            public int Id_Ciclo
            {
                get; set;
            }
            
            public bool ArquivoGeral
            {
                get; set;
            }

        }
    }
