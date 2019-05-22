using System;
using System.Threading;

namespace ArchiveAutoload
{
    // => - запрос
    // | - следущая задача

    // Server дай мне временную метку самого старого файла =>

    // VideoReg ищет в архиве самый новый файл(file) возвращает его дату(file.date) 
    // (поиск и дата берутся из имени файла, целесообразно вести кеш архива и обновлять его раз в минуту) =>

    // Server устанавливает Mmax и Mmin метки для данного регистратора в БД |

    // Server дай мне файл у последние файлы архива  => 

    // VideoReg ищем файлы у которых Min(Mmax <= file.date)
    // если таких нет то верни (Mmin >= file.date)
    // также добавим заголовк fileDate c датой файла из его имени
    // если и таких нет то statuscode 404 =>

    // Server в зависимости от даты которая пришла определяет к какой метке относится дата
    // if(fileDate >= Mmax) =>  Mmax else => Mmin 

    public class ArchiveAutoloader
    {
        
    }


    public class ArhiveResponce<T>
    {
        public DateTime Date { get; set; }
        public T Archive { get; set; }
    }

    public class IveArchive { }
    public class CameraArchive { }

    public class RequestParameter
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class RequestOptions
    {
        public int Timeout { get; set; } = 3000;
        public CancellationToken Token { get; set; }
        public RequestParameter[] Parameters { get; set; }
    }

    public interface IArviceApi
    {
        DateTime GetLastArchiveDate();
        ArhiveResponce<RequestOptions> GetCameraArhiveResponce(DateTime Mmax, DateTime Mmin, RequestOptions options);
    }
}
