# CoinMapApiTools

This tool calls the CoinMap API to get the list of all venues with details and print to CSV.

## Using Tool

1. Update configuration, if needed
	* uri -> Provide the URI to the CoinMap API; including the path to the venues.
	* output -> Provide the output directory that the program will print the CSV. The file is hardcoded to be named 'Venues.csv' and will overwrite the file if one exists already.
2. Execute tool
	* The program will execute with no additional input and will print to the console window and will require an <enter> to exit.
	* This takes a little bit of time to execute (10-20 mins) because it is currently querying for every venue to get more venue details.