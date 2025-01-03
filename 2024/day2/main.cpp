#include <cstdint>
#include <cstdio>
#include <cstdlib>
#include <sys/stat.h>
#include <vector>

void clear(char* array, uint8_t size){
    for(int i = 0; i < size; i++){
        array[i] = '\0';
    };
}

uint8_t isSafe(std::vector<int> line){

    uint8_t safe = 1;
    int sign = 0;
    for(int j = 0; j < line.size() - 1; j++){
        int diff = line[j+1] - line[j]; 
        uint absDiff = 0;
        if (diff < 0){
            absDiff = -diff;
        }
        else{
            absDiff = diff;
        }
        if (absDiff < 1 || absDiff > 3)
        {
            safe = 0;
            break;
        }
        if (sign == 0)
        {
            if (diff < 0){
                sign = -1;
            }
            else{
                sign = 1;
            }
        }
        else
        {
            int newSign = 0;
            if (diff < 0){
                newSign = -1;
            }
            else{
                newSign = 1;
            }
            if (newSign != sign){
                safe = 0;
                break;
            }
        }
    }
    return safe;
}
std::vector<std::vector<int>>* parseFile(uint8_t *data, uint size){
    char stringNumber[6] = {'\0'}; // supporting only numbers with max value 99999 (last 6th character has to be \0
    uint8_t charIndex = 0;
    uint8_t readingNumber = 1;

    std::vector<std::vector<int>> *numbers = new std::vector<std::vector<int>>;
    std::vector<int> *line = new std::vector<int>;

    for(uint i = 0; i < size; i++){
        char item = data[i];
        if (item == ' ' && readingNumber){
            int number = atoi(stringNumber);
            line->push_back(number);
            clear(stringNumber, 6);
            readingNumber = 0;
            charIndex = 0;
        }
        else if(item == ' '){

        }
        else if (item == '\n'){
            int number = atoi(stringNumber);
            line->push_back(number);
            numbers->push_back(*line); // perhaps this copies the vector?
            line = new std::vector<int>;
            readingNumber = 1;
            clear(stringNumber, 6);
            charIndex = 0;
        }
        else{
            stringNumber[charIndex] = item;
            charIndex++;
            readingNumber = 1;
        }
    }

    return numbers;
}

int main(int argc, char* argv[])
{
    if (argc != 2){
        printf("usage: day2 [input file]\n");
        return -1;
    }
    const char* fileName = argv[1];

    FILE* filePointer = fopen(fileName, "r");

    struct stat fileStat;
    stat(fileName, &fileStat);
    uint8_t *data = (uint8_t *)malloc(fileStat.st_size);
    fread(data, fileStat.st_size, 1, filePointer);

    std::vector<std::vector<int>> *lines = parseFile(data, fileStat.st_size);
    printf("ROWS: %ld\n", lines->size());

    uint safeCounter = 0;
    for(int i = 0; i < lines->size(); i++){

        std::vector<int> line = lines->at(i);
        if (isSafe(line)){
            safeCounter++;
        }
    }

    printf("%d\n", safeCounter);
}
