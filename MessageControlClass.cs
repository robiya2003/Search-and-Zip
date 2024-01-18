using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO.Compression;using System.IO;


namespace Search_and_Zip
{

    public class MessageControlClass
    {
        public static long _id;
        public static string _messageText;
        public static bool search=false;
        public static bool zip=false;

        public static async Task EssentialFunctionMessage(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _id=update.Message.Chat.Id;
            var message = update.Message.Type switch
            {
                MessageType.Text => TextFunction(botClient, update, cancellationToken),
                _=>OtherFunction(botClient, update, cancellationToken)

            };
            
        }

        static async Task TextFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _messageText = update.Message.Text;
            if(_messageText == "/start")
            {
                StartButton(botClient, update, cancellationToken);
                search=false;
                zip=false;

    }
            else if(_messageText == "Search File")
            {
                await botClient.SendTextMessageAsync(
               chatId: _id,
               text: "Fayl nomini kiriting",
               cancellationToken: cancellationToken);
                search = true;
                zip = false;
            }
            else if(_messageText == "Zip Folder")
            {
                await botClient.SendTextMessageAsync(
               chatId: _id,
               text: "Papka nomini kiriting",
               cancellationToken: cancellationToken);
                zip = true;
                search = false;

            }
            else
            {
                if(search)
                {
                    SearchFile searchFile = new SearchFile(_messageText);
                    var Result = searchFile.ResultReturn();
                    if (Result.Count == 0)
                    {
                        await botClient.SendTextMessageAsync(
                           chatId: _id,
                           text: "Topilmadi",
                           cancellationToken: cancellationToken);
                    }
                    else
                    {
                        foreach (string f in Result)
                        {
                            await botClient.SendTextMessageAsync(
                           chatId: _id,
                           text: f,
                           cancellationToken: cancellationToken);
                        }
                        StartButton(botClient, update, cancellationToken);

                    }
                }
                else if (zip)
                {
                  
                    string startPath =_messageText;

                    

                    string zipPath = @"C:\Users\LENOVO\Desktop\zip files\result.zip";
                    FileInfo fileInfo = new FileInfo(zipPath);
                    fileInfo.Delete();
                    ZipFile.CreateFromDirectory(startPath, zipPath);
                    
                    await using Stream stream = System.IO.File.OpenRead(zipPath);
                    
                    Message message = await botClient.SendDocumentAsync(
                        chatId: _id,
                        document: InputFile.FromStream(stream:stream,fileName:"result.zip"));

                    Console.WriteLine("Code came here");
                    stream.Close();
                    await Task.Delay(3000);
                    
                    fileInfo.Delete();
                    Console.WriteLine("Must be succesfully delete zip file.");
                }
                
                else
                {
                    
                }
                
                
            }
        }
        #region
        static async Task OtherFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            return;
        }
        public static async Task StartButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new[]
            {
                new KeyboardButton[] { "Search File" },new KeyboardButton[] { "Zip Folder" },
            }
                )

            {
                ResizeKeyboard = true
            };


                await botClient.SendTextMessageAsync(
                chatId: _id,
                text: "Operatsiyani tanlang",
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken);
        }
        #endregion
    }
}
