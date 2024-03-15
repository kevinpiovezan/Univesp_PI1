using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Services
{
    public static class CompressorArquivo
    {
        // public static MemoryStream Zip(List<ArquivoDTO> arquivos)
        // {
        //     var arquivoZipStream = new MemoryStream();
        //
        //     using (var zipArchive = new ZipArchive(arquivoZipStream, ZipArchiveMode.Create, true))
        //     {
        //         foreach (var arquivo in arquivos)
        //         {
        //             // var arquivoBase64 = arquivo.Base64.Split(new[] { ";base64," }, StringSplitOptions.RemoveEmptyEntries)[1];
        //
        //             var arquivoComprimido = zipArchive.CreateEntry(arquivo.Nome, CompressionLevel.Optimal);
        //
        //             using var entryStream = arquivoComprimido.Open();
        //             byte[] arquivoEmBytes = Convert.FromBase64String(arquivo.Base64);
        //
        //             using var arquivoAComprimirStream = new MemoryStream(arquivoEmBytes);
        //             arquivoAComprimirStream.CopyTo(entryStream);
        //         }
        //     }
        //     arquivoZipStream.Position = 0;
        //     return arquivoZipStream;
        // }
    }
}