#include "../parser.cpp"

int main(int argc, char* argv[])
{
    File file = loadFile(argv[1]);
    for (int i = 0; i < file.size; i++){
        printf("%c", file.data[i]);
    }
    return 0;
}
