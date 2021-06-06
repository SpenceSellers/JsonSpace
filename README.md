# JsonSpace
How many valid JSON strings fit in 100 characters? What does a statistically random JSON value look like? This project seeks to find answers to these gripping questions and more.

## Results: How many valid JSON strings are there of length N?
To narrow the scope of the question, I'm only considering the 95 characters that can be typed on a standard English keyboard. The JSON strings being counted may certainly contain escaped unicode characters, but they won't contain them in their raw form.

| Length | Valid JSON strings | Size of string space | Percent valid | Time to compute |
| --- | --- | --- | --- | --- |
| 0 | 0 | 1 | 0% | ~0ms |
| 1 | 10 | 95 | 11.11111% | 1ms |
| 2 | 123 | 9,025 | 1.36987% | 3ms |
| 3 | 1,631 | 857,375 | 0.19048% | ~0ms |
| 4 | 27,823 | 81,450,625 | 0.0341647% | 1ms |
| 5 | 1,052,294 | 7,737,809,375 | 0.01359989% | 1ms |
| 6 | 79,363,901 | 735,091,890,625 | 0.0107968% | 6ms |
| 7 | 7,167,426,599 | 69,833,729,609,375 | 0.010263779% | 25ms |
| 8 | 664,619,637,035 | 6,634,204,312,890,625 | 0.010019036% | 145ms |
| 9 | 61,837,230,111,568 | 630,249,409,724,609,375 | 0.00981161695% | 607ms |
| 10 | 5,755,842,691,968,125 | 59,873,693,923,837,890,625 | 0.0096135359 | 2598ms |
| 11 | 535,784,439,899,095,505 | 5,688,000,922,764,599,609,375 | 0.0094197437 | 9838ms |
| 12 | 49,873,974,843,674,606,335 | 540,360,087,662,636,962,890,625 | 0.009230201218% | 38655ms |
