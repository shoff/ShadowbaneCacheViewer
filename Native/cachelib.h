
#pragma once

#include "stdafx.h"
#include <iostream>
#include <string>
#include <fstream>
using namespace std;
typedef unsigned __int32 uint32;
typedef __int32 int32;
typedef unsigned __int16 uint16;
typedef __int16 int16;

// Forward declaration
class CacheArchive;

// Archives
extern CacheArchive cArchive;
extern CacheArchive texArchive;
extern CacheArchive meshArchive;
extern CacheArchive objArchive;
extern CacheArchive renderArchive;
extern CacheArchive skeletonArchive;
extern CacheArchive motionArchive;
extern CacheArchive zoneArchive;


struct CacheHeader 
{
	uint32 indexCount;	// Number of files?
    uint32 dataOffset;	// File offset to where the data chunks begin
	uint32 fileSize;	// total size of the file
    uint32 junk1;		// 0xFFFF ffff
};

struct CacheIndex 
{
	uint32 junk1;		// 0x0000 0000
	uint32 id;			// index value
    uint32 offset;		// file offset to data chunk for this index
	uint32 rawSize;		// uncompressed size of data
    uint32 size;		// compressed size of data
};

// The Data chunks in cache files are zlib compressed.

class CacheArchive
{
private:
	std::ifstream cacheFile;
	CacheHeader header;
	CacheIndex *index;
	unsigned char *buffer;
	char *outBuffer;

public:
	CacheArchive();
	CacheArchive(const wxChar* filename);
	~CacheArchive();

	unsigned int getFileCount();
	unsigned char* getFile(unsigned int fileIndex);
	unsigned int getFileSize(unsigned int fileIndex);
	unsigned char* getFileByID(unsigned int id);
	unsigned int getFileID(unsigned int fileIndex);
	unsigned int getFileSizeByID(unsigned int id);
	void close();
	void open(const wxChar* filename);
};