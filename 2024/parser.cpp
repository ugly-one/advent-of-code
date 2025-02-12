#include <cstdint>
#include <cstdio>
#include <cstdlib>
#include <vector>
#include <sys/stat.h>

struct File{
    uint8_t* data;
    int size;
};

bool loadFile(const char* fileName, File *file){

    FILE *filePointer = fopen(fileName, "r");

    if (filePointer == NULL){
        return false;
    }

    struct stat fileStat;
    stat(fileName, &fileStat);
    uint8_t *data = (uint8_t *)malloc(fileStat.st_size);
    fread(data, fileStat.st_size, 1, filePointer);

    file->size = fileStat.st_size;
    file->data = data;

    return file;
}

void clear(char* array, uint8_t size){
    for(int i = 0; i < size; i++){
        array[i] = '\0';
    };
}

std::vector<int> *parseLine(uint8_t *&data, char separator){
    char stringNumber[6] = {'\0'}; // supporting only numbers with max value 99999 (last 6th character has to be \0
    uint8_t charIndex = 0;
    uint8_t addToResult = 0;

    std::vector<int> *line = new std::vector<int>;

    while((*data) != '\n'){
        char item = *data;
        if (item == separator && addToResult){
            int number = atoi(stringNumber);
            line->push_back(number);
            clear(stringNumber, 6);
            addToResult = 0;
            charIndex = 0;
        }
        else{
            stringNumber[charIndex] = item;
            charIndex++;
            addToResult = 1;
        }
        data++;
    }
    if (addToResult){
        int number = atoi(stringNumber);
        line->push_back(number);
    }
    data++;
    return line;
}


std::vector<std::vector<int>>* parseFile(uint8_t *data, uint size){
    char stringNumber[6] = {'\0'}; // supporting only numbers with max value 99999 (last 6th character has to be \0
    uint8_t charIndex = 0;
    uint8_t readingNumber = 1;

    std::vector<std::vector<int>> *numbers = new std::vector<std::vector<int>>;
    std::vector<int> *line = new std::vector<int>;
    
    // todo make this function use parseLine
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
