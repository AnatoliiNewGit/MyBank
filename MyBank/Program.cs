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
                        Authorization(ref authorization);
                        break;
                    case 2:
                        return;
                    default:
                        Console.WriteLine("Неверная команда. Попробуйте еще.");
                        break;
                }
            } while (true);
        }
        static void Authorization(ref string[,] authorization)
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
                        if (login == "Admin" && password == authorization[i, 1])
                        {
                            if (AdminMenu(ref authorization))
                            {
                                return;
                            }
                        }
                        else if (password == authorization[i, 1])
                        {
                            if(UserMenu(authorization, login))
                            {
                                return;
                            }
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
                Console.WriteLine("Попробовать еще раз? (Enter/нет)");
                if (Console.ReadLine() == "нет")
                {
                    return;
                }
            } while (true);
        }
        static bool AdminMenu(ref string[,] authorization)
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
                        BankAccountUnblocked(ref authorization);
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
        static void ShowListAccount(string[,] authorization)
        {
            if (authorization.GetLength(0) > 1)
            {
                Console.WriteLine("Имя | " + "Счет | " + "Статус");
                for (int i = 1; i < authorization.GetLength(0); i++)
                {
                    Console.WriteLine("{0} | {1} | {2}", authorization[i, 0], authorization[i, 2], authorization[i, 3]);
                }
            }
            else
            {
                Console.WriteLine("Список клиентов пуст.");
            }
        }
        static void BankAccountBlocked(ref string[,] authorization)
        {
            int index;
            do
            {
                Console.WriteLine("Введите имя пользователя для блокировки счета:");
                index = ExistName(authorization, Console.ReadLine());
                if (index > -1)
                {
                    BlockedUnblocked(authorization, index, true);
                    return;
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем не найдено. Попробовать еще раз? (Enter/нет)");
                    if (Console.ReadLine() == "нет")
                    {
                        return;
                    }
                }
            } while (true);
        }
        static void BankAccountUnblocked(ref string[,] authorization)
        {
            int index;
            do
            {
                Console.WriteLine("Введите имя пользователя для разблокировки счета:");
                index = ExistName(authorization, Console.ReadLine());
                if (index > -1)
                {
                    BlockedUnblocked(authorization, index, false);
                    return;
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем не найдено. Попробовоть еще раз? (Enter/нет)");
                    if (Console.ReadLine() == "нет")
                    {
                        return;
                    }
                }
            } while (true);
        }
        static void BlockedUnblocked(string[,] authorization, int index, bool isBlocked)
        {
            if (isBlocked)
            {
                authorization[index, 3] = "Blocked";
                Console.WriteLine("Пользователь {0} заблокирован.", authorization[index, 0]);
            }
            else if (!isBlocked)
            {
                authorization[index, 3] = "Unblocked";
                Console.WriteLine("Пользователь {0} разблокирован.", authorization[index, 0]);
            }
        }
        static string[,] AddNewBankAccount(string[,] authorization)
        {
            string[,] newAuthorization = new string[authorization.GetLength(0) + 1, 4];
            string userName = null;
            string userAccount = null;
            int index;
            do
            {
                Console.WriteLine("Введите имя нового пользователя:");
                userName = Console.ReadLine();
                index = ExistName(authorization, userName);
                if (index == -1)
                {
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
                    newAuthorization[newAuthorization.GetLength(0) - 1, 1] = userName;
                    newAuthorization[newAuthorization.GetLength(0) - 1, 2] = userAccount;
                    newAuthorization[newAuthorization.GetLength(0) - 1, 3] = "Unblocked";
                    return newAuthorization;
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем уже существует. Попробовоть другое имя? (Enter/нет)");
                    if (Console.ReadLine() == "нет")
                    {
                        return authorization;
                    }
                }
            } while (true);
        }
        static string[,] BankAccountDelete(string[,] authorization)
        {
            string[,] newAuthorization = new string[authorization.GetLength(0) - 1, 4];
            int newIndex = 0;
            int index;
            do
            {
                Console.WriteLine("Введите имя пользователя для удаления счета:");
                index = ExistName(authorization, Console.ReadLine());
                if (index > -1)
                {
                    for (int i = 0; i < authorization.GetLength(0); i++)
                    {
                        if (i == index)
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
                    Console.WriteLine("Пользователь {0} удален.", authorization[index, 0]);
                    return newAuthorization;
                }
                else
                {
                    Console.WriteLine("Клиента с таким именем не найдено. Попробовать еще раз? (Enter/нет)");
                    if (Console.ReadLine() == "нет")
                    {
                        return authorization;
                    }
                }
            } while (true);
        }
        
        static int ExistName(string[,] authorization, string name)
        {
            for (int i = 1; i < authorization.GetLength(0); i++)
            {
                if (authorization[i, 0] == name)
                {
                    return i;
                }
            }
            return -1;
        }

        static bool UserMenu(string[,] authorization, string userName)
        {
            string action = null;
            do
            {
                Console.WriteLine("Выберите действие: \n 1-Просмотреть счет \n 2-Пополнить счет \n 3-Снять деньги со счета \n 4-Вернуться в главное меню");
                action = Console.ReadLine();
                switch (action)
                {
                    case "1":
                        Console.WriteLine("Метод просмотра счета");
                        ShowBalance(authorization, userName);
                        break;
                    case "2":
                        Console.WriteLine("Метод пополнения счета");
                        AccountReplenish(ref authorization, userName);
                        break;
                    case "3":
                        Console.WriteLine("Метод снятия денег со счета");
                        CashWithdrawal(ref authorization, userName);
                        break;
                    case "4":
                        return true;
                    default:
                        Console.WriteLine("Неизвестная операция");
                        break;
                }
            } while (true);
        }
        static void ShowBalance(string[,] authorization, string userName)
        {
            for (int i = 0; i < authorization.GetLength(0); i++)
            {
                if(authorization[i, 0] == userName)
                {
                    Console.WriteLine("Ваш баланс = {0}.", authorization[i, 2]);
                }
            }
        }
        static void AccountReplenish(ref string[,] authorization, string userName)
        {
            int money = 0;
            for (int i = 0; i < authorization.GetLength(0); i++)
            {
                if(authorization[i, 0] == userName)
                {
                    do
                    {
                        Console.WriteLine("Введите сумму для пополнения:");
                        money = int.Parse(Console.ReadLine());
                        if (money > 0)
                        {
                            authorization[i, 2] = (int.Parse(authorization[i, 2]) + money).ToString();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Не корректная сумма! Попробовать еще раз? (Enter/нет)");
                            if (Console.ReadLine() == "нет")
                            {
                                return;
                            }
                        }
                    } while (true);
                }
            }
        }
        static void CashWithdrawal(ref string[,] authorization, string userName)
        {
            int money = 0;
            for (int i = 0; i < authorization.GetLength(0); i++)
            {
                if (authorization[i, 0] == userName)
                {
                    do
                    {
                        Console.WriteLine("Введите сумму для снятия:");
                        money = int.Parse(Console.ReadLine());
                        if (money > 0 && money <= int.Parse(authorization[i, 2]))
                        {
                            authorization[i, 2] = (int.Parse(authorization[i, 2]) - money).ToString();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Не корректная сумма! На вашем счету {0}. Попробовать еще раз? (Enter/нет)", authorization[i, 2]);
                            if (Console.ReadLine() == "нет")
                            {
                                return;
                            }
                        }
                    } while (true);
                }
            }
        }
    }
}
