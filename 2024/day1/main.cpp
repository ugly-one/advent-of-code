#include <cstdint>
#include <cstdio>
#include <cstdlib>
#include <sys/stat.h>

int comp(const void* a, const void* b) {
    return (*(int*)a - *(int*)b);
}

int main(int argc, char* argv[])
{
    if (argc != 3){
        printf("usage: day1 [input file] [number of lines in the file]\n");
        return -1;
    }
    const char* fileName = argv[1];
    const uint size = atoi(argv[2]);

    FILE* filePointer = fopen(fileName, "r");
    struct stat fileStat;
    stat(fileName, &fileStat);
    uint8_t *data = (uint8_t *)malloc(fileStat.st_size);
    fread(data, fileStat.st_size, 1, filePointer);

    char string[6] = {'\0'};
    uint8_t charIndex = 0;
    uint8_t readingFirstNumber = 1;

    int* left = (int*) malloc(size * sizeof(int));
    int* right = (int*) malloc(size * sizeof(int));
    int itemIndex = 0;
    for(int i = 0; i< fileStat.st_size; i++){
        if (data[i] == ' ' && readingFirstNumber){
            int n1 = atoi(string);
            left[itemIndex] = n1;
            readingFirstNumber = 0;
            charIndex = 0;
        }
        else if(data[i] == ' '){

        }
        else if (data[i] == '\n'){
            int n2 = atoi(string);
            right[itemIndex] = n2;
            readingFirstNumber = 1;
            charIndex = 0;
            itemIndex++;
        }
        else{
            string[charIndex] = data[i];
            charIndex++;
        }
    }

    qsort(left, size, sizeof(int), comp);
    qsort(right, size, sizeof(int), comp);

    uint sum = 0;
    for(int i = 0; i < size; i++)
    {
        int leftItem = left[i];
        int rightItem = right[i];
        int diff = leftItem - rightItem;
        if (diff < 0){
            diff = diff * -1;
        }

        sum += diff;
    }

    printf("%d", sum);
}
