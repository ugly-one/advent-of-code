#include <stdio.h>
#include <stdlib.h>

int main() {
  FILE *file = fopen("../../../advent-of-code-input/2025/day1/input.txt", "r");
  if (file == NULL)
    return -1;

  int position = 50;
  int answer = 0;

  char buffer[256];
  while (fgets(buffer, sizeof(buffer), file)) {
    char direction = buffer[0];
    int count = atoi(&buffer[1]);

    for (int i = 0; i < count; i++) {
      if (direction == 'L') {
        position--;
      } else {
        position++;
      }

      if (position < 0) {
        position = 99;
      } else if (position > 99){
				position = 0;
			}

			if (position == 0){
				answer++;
			}
    }
  }
  fclose(file);
  printf("%d\n", answer);
  return 0;
}
