using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Nethereum.Web3;
using System.Linq;

public class Program
{
    private static readonly Web3 Web3 = new Web3("https://eth-mainnet.alchemyapi.io/v2/SWnnr6RA00IpzVBqxszTH44af_QbWrN1");
    private const string EthFile = "eth.txt";
    private const string HighBalanceFile = "high_balance.txt";
    private const decimal MinBalanceUsd = 1000; // Set the minimum balance value
    public const bool removeCheckedAddresses = true; // Set to true to remove checked addresses from eth.txt


    static async Task<decimal> GetEthPriceAsync()
    {
        using (var webClient = new WebClient())
        {
            var response = await webClient.DownloadStringTaskAsync("https://min-api.cryptocompare.com/data/price?fsym=ETH&tsyms=USD");
            dynamic data = JsonConvert.DeserializeObject(response);
            return Convert.ToDecimal(data["USD"]);
        }
    }

    public static async Task<decimal> GetEthBalanceAsync(string address)
    {
        var balanceWei = await Web3.Eth.GetBalance.SendRequestAsync(address);
        return Web3.Convert.FromWei(balanceWei);
    }

    public static void ProcessAddress(string address, ref int highBalanceCount, ref int totalAddressesChecked, decimal ethPrice, Stopwatch stopwatch)
    {
        decimal balanceEth = GetEthBalanceAsync(address).Result;
        decimal balanceDollar = balanceEth * ethPrice;

        if (balanceDollar > MinBalanceUsd)
        {
            if (!File.Exists(HighBalanceFile))
            {
                File.WriteAllText(HighBalanceFile, "");
            }

            string fileContent = File.ReadAllText(HighBalanceFile);
            if (!fileContent.Contains(address))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"#{totalAddressesChecked} | Address: {address} | {balanceEth:N18} ETH | ${balanceDollar:N2}");
                Console.ForegroundColor = ConsoleColor.White;
                File.AppendAllText(HighBalanceFile, address + Environment.NewLine);
                highBalanceCount++;
            }
        }
        else
        {
            Console.WriteLine($"#{totalAddressesChecked} | Address: {address} | {balanceEth:N18} ETH | ${balanceDollar:N2}");
        }

        totalAddressesChecked++;

        if (removeCheckedAddresses)
        {
            // Remove the processed address from the file
            string[] lines = File.ReadAllLines(EthFile);
            File.WriteAllLines(EthFile, lines.Skip(1).ToArray());
        }

        TimeSpan elapsed = stopwatch.Elapsed;
        Console.Title = $"High Balances: {highBalanceCount} | Total Checked: {totalAddressesChecked} | Remaining: {File.ReadLines(EthFile).Count()} | Runtime: {elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}";
    }

    public static async Task Main(string[] args)
    {
        try
        {
            if (!File.Exists(EthFile))
            {
                Console.WriteLine($"File '{EthFile}' does not exist. Creating a new file...");
                File.WriteAllText(EthFile, "");
                Console.WriteLine("File created. Please add Ethereum addresses to the file and run the program again.");
                Console.ReadKey();
                return;
            }

            Console.Title = "Checking Ethereum Addresses";

            var ethPrice = await GetEthPriceAsync();
            int highBalanceCount = 0;
            int totalAddressesChecked = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                var addresses = File.ReadAllLines(EthFile);

                if (addresses.Length == 0)
                {
                    Console.WriteLine("The file 'eth.txt' is empty.");
                    Console.ReadKey();
                    continue;
                }

                for (int i = 0; i < addresses.Length; i++)
                {
                    ProcessAddress(addresses[i].Trim(), ref highBalanceCount, ref totalAddressesChecked, ethPrice, stopwatch);
                }

                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
