namespace Syngenta.Casa.RenCad.ApplicationCore.Enums
{
    public enum EStatusClienteCiclo
    {
        PENDENTE = 1,
        RECEBIDO_INCOMPLETO = 2,
        ENV_P_CREDITO = 3,
        ENV_P_CREDITO_INCOMPLETO = 4,
        RECEBIDO_COMPLETO = 6,
        NAO_RENOVAR = 99, //NÃO EXISTE NO BANCO
    }
}