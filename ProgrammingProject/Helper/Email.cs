using Azure;
using Azure.Communication.Email;


namespace ProgrammingProject.Helper
{
    public class Email
    {
        public static async void SendEmail(string recipient, string subject, string htmlContent)
        {
            // This code demonstrates how to send email using Azure Communication Services.
            var key = new AzureKeyCredential("IU/GCS0JXhqayxC5CSFJ7Q04aw9wZEU27HqTdVXyCXhnFbPfkK6Fn1Ho7z0etqoxYVwbCI4vAVvwN9r0TlNkOw==");
            var endpoint = new Uri("https://easywalkcommunicationservice.communication.azure.com/");
            var emailClient = new EmailClient(endpoint, key);

            var sender = "DoNotReply@09c25cdc-741a-4d66-bdd5-d6b5774ae494.azurecomm.net";
           
            try
            {
                var emailSendOperation = await emailClient.SendAsync(
                    wait: WaitUntil.Completed,
                    senderAddress: sender, // The email address of the domain registered with the Communication Services resource
                    recipientAddress: recipient,
                    subject: subject,
                    htmlContent: htmlContent);
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }

    }
}
