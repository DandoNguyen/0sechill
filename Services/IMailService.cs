using _0sechill.Dto.MailDto;

namespace _0sechill.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailContent mailContent);
    }
}
