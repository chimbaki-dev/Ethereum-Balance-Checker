# Ethereum Wallet Balances Checker

This project is a C# script that allows you to search for balances of Ethereum wallets and display information about them in the console. Additionally, the script saves the addresses of wallets with a minimum balance to a separate file. The script utilizes public APIs to retrieve information about balances and current ETH prices.  

## Installation

1. Install the [.NET Core SDK](https://dotnet.microsoft.com/download) on your computer.
2. Navigate to the project directory:
3. Open the solution file (.sln).
4. Compile the project from the Build menu.

## Usage

1. Enter the addresses in the `eth.txt` file inside the `.\Checker` folder.
2. Add Ethereum addresses to the `eth.txt` file, with each address on a separate line.
3. Run the application.

## Requirements
- .NET Core 3.1 or higher version
- [Newtonsoft.Json](https://www.newtonsoft.com/json): A popular JSON framework for .NET.
- [Nethereum.Web3](https://nethereum.github.io/): The .NET Ethereum library that interacts with the Ethereum blockchain.


## File Descriptions

- `Program.cs`: Source code of the C# application.
- `eth.txt`: File containing Ethereum addresses to check balances.
- `high_balance.txt`: File where addresses with balances exceeding $1000 (by default) are logged.

## Additional Functionality

### Removing Checked Addresses

The script includes a feature to remove checked Ethereum addresses from the `eth.txt` file. By default, this feature is enabled (`removeCheckedAddresses = true`). When an address is successfully processed and its balance is displayed, it is automatically removed from the `eth.txt` file to avoid redundant checks.

If you prefer to keep the addresses in the file even after they have been checked, you can set `removeCheckedAddresses` to `false` in the source code.


## API Requirements

The script requires access to the following public APIs:
- [Alchemy API](https://alchemyapi.io/) for retrieving Ethereum balance information.
- [CryptoCompare API](https://min-api.cryptocompare.com/) for fetching current ETH prices.

## Important

- Use this script only in accordance with the terms of use of the respective APIs ([Alchemy](https://alchemy.com/terms), [CryptoCompare](https://min-api.cryptocompare.com/terms)).
- The author are not responsible for any misuse of the script or any potential losses associated with its use.

## License

This project is licensed under the [MIT License](LICENSE).

