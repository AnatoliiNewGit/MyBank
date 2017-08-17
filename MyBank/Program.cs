using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu();
        }

        static void MainMenu()
        {
            string[,] authorization = new string[1, 4] { { "Admin", "admin", "0", "0" } };
            int action = 0;
            do
            {
                Console.WriteLine("Выберите действие: \n  1 - Вход \n  2 - Выход");
                action = int.Parse(Console.ReadLine());
                switch (action)
                {
                    case 1:
                        Authorization(authorization);
                        break;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("Неверная команда. Попробуйте еще.");
                        break;
                }
            } while (true);
        }
        static void Authorization(string[,] authorization)
        {
            string login = null, password = null;

            bool isLogin = false;
            do
            {
                Console.WriteLine("Введите логин");
                login = Console.ReadLine();
                Console.WriteLine("Введите пароль");
                password = Console.ReadLine();
                for (int i = 0; i < authorization.GetLength(0); i++)
                {
                    if (login == authorization[i, 0])
                    {
                        isLogin = true;
                        if (password == authorization[i, 1] && login == "Admin")
                        {
                            if (AdminMenu(authorization))
                            {
                                return;
                            }
                        }
                        else if (password == authorization[i, 1] && login == "User")
                        {
                            Console.WriteLine("Меню пользователя.");
                        }
                        else
                        {
                            Console.WriteLine("Пароль не правильный. Попробуйте еще раз. \n");
                            break;
                        }
                    }
                }
                if (!isLogin)
                {
                    Console.WriteLine("Логин не правильный. Попробуйте еще. \n");
                }
                Console.WriteLine("Вернуться в главное меню? да/нет");
                if (Console.ReadLine() == "да")
                {
                    return;
                }
            } while (true);
        }
        static bool AdminMenu(string[,] authorization)
        {
            string action = null;
            do
            {
                Console.WriteLine("Выберите действие: \n 1-Просмотреть список счетов \n 2-Блокировать пользователя \n 3-Разблокировать пользователя \n 4-Добавить новый счет \n 5-Удалить существующий счет \n 6-Вернуться в главное меню");
                action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        ShowListAccount(authorization);
                        break;
                    case "2":
                        BankAccountBlocked(ref authorization);
                        break;
                    case "3":
                        Console.WriteLine("Метод разблокировки пользователя");
                        break;
                    case "4":
                        authorization = AddNewBankAccount(authorization);
                        break;
                    case "5":
                        authorization = BankAccountDelete(authorization);
                        break;
                    case "6":
                        return true;
                    default:
                        Console.WriteLine("Неизвестная операция");
                        break;
                }
            } while (true);
        }
        static void BankAccountBlocked(ref string[,] authorization)
        {
            string userName = null;
            do
            {
                Console.WriteLine("Введите имя пользователя для блокировки счета:");
                userName = Console.ReadLine();
                if (ExistName(authorization, userName))
                {
                    for (int i = 1; i < authorization.GetLength(0); i++)
                    {
                        if (authorization[i, 0] == userName && authorization[i, 3] != "blocked")
                        {
                            authorization[i, 3] = "blocked";
                            Console.WriteLine("Пользователь {0} заблокирован.", authorization[i, 0]);
                            return;
                        }
                        else if (authorization[i, 0] == userName && authorization[i, 3] == "blocked")
                        {
                            Console.WriteLine("Пользователь {0} уже заблокирован.", authorization[i, 0]);
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем не найдено. Попробовоть еще раз? (да/нет)");
                    if (Console.ReadLine() != "да")
                    {
                        return;
                    }
                }
            } while (true);
        }
        static string[,] AddNewBankAccount(string[,] authorization)
        {
            string[,] newAuthorization = new string[authorization.GetLength(0) + 1, 4];
            string userName = null;
            string userAccount = null;
            Console.WriteLine("Введите имя нового пользователя:");
            userName = Console.ReadLine();
            Console.WriteLine("Введите счет пользователя:");
            userAccount = Console.ReadLine();

            for (int i = 0; i < authorization.GetLength(0); i++)
            {
                for (int j = 0; j < authorization.GetLength(1); j++)
                {
                    newAuthorization[i, j] = authorization[i, j];
                }
            }
            newAuthorization[newAuthorization.GetLength(0) - 1, 0] = userName;
            newAuthorization[newAuthorization.GetLength(0) - 1, 2] = userAccount;
            newAuthorization[newAuthorization.GetLength(0) - 1, 3] = "unblocked";
            return newAuthorization;
        }
        static string[,] BankAccountDelete(string[,] authorization)
        {
            string[,] newAuthorization = new string[authorization.GetLength(0) - 1, 4];
            string userName = null;
            int newIndex = 0;
            do
            {
                Console.WriteLine("Введите имя пользователя для удаления счета:");
                userName = Console.ReadLine();
                if (ExistName(authorization, userName))
                {
                    for (int i = 0; i < authorization.GetLength(0); i++)
                    {
                        if (userName != "Admin" && authorization[i, 0] == userName)
                        {
                            continue;
                        }
                        else
                        {
                            for (int j = 0; j < authorization.GetLength(1); j++)
                            {
                                newAuthorization[newIndex, j] = authorization[i, j];
                            }
                            newIndex++;
                        }
                    }
                    return newAuthorization;
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем не найдено. Попробовоть еще раз? (да/нет)");
                    if (Console.ReadLine() != "да")
                    {
                        return authorization;
                    }
                }
            } while (true);
        }
        static void ShowListAccount(string[,] authorization)
        {
            Console.WriteLine("Имя | " + "Счет | " + "Статус");
            for (int i = 0; i < authorization.GetLength(0); i++)
            {
                if (authorization[i, 0] != "Admin")
                {
                    Console.WriteLine("{0} | {1} | {2}", authorization[i, 0], authorization[i, 2], authorization[i, 3]);
                }
            }
        }

        static bool ExistName(string[,] authorization, string name)
        {
            for (int i = 1; i < authorization.GetLength(0); i++)
            {
                if (authorization[i, 0] == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
