
#include "stdafx.h"
#include "cachelib.h"
#include "ximage.h"
#include <string.h>


// Archives
CacheArchive cArchive;
CacheArchive texArchive;
CacheArchive meshArchive;
CacheArchive objArchive;
CacheArchive renderArchive;
CacheArchive skeletonArchive;
CacheArchive motionArchive;
CacheArchive zoneArchive;

CacheArchive::CacheArchive()
{
	index = 0;
	buffer = 0;
	outBuffer = 0;
}

CacheArchive::CacheArchive(const wxChar *filename)
{
	index = 0;
	buffer = 0;
	outBuffer = 0;
	open(filename);
}

void CacheArchive::open(const wxChar *filename)
{
	// If the archive still has a file open from previously, close it first!
	if (cacheFile.is_open())
	{
		close();
	}

	// Open cache file,  mode: input, binary, position at end of file
	wxString fn = filename;
	cacheFile.open(fn.mb_str(), std::ios::in|std::ios::binary|std::ios::ate);

	if (!cacheFile.is_open()) 
	{
		wxLogMessage(_T("Error: unable to open %s."), filename);
		return;
	}
	
	header.dataOffset = 0;
	header.fileSize = 0;
	header.indexCount = 0;

	// Get file size.
	int size = cacheFile.tellg();

	// Set position back to start of file
	cacheFile.seekg(0, std::ios::beg);

	// Read in the header data from file.
	cacheFile.read((char*)&header, sizeof(CacheHeader));

	wxLogMessage(_T("SizeOf CacheHeader: %i"), sizeof(CacheHeader));

	if (size != header.fileSize) 
	{
		// Error file size mismatch!
		wxLogMessage(_T("Error: Filesize mismatch!"));
		return;
	}

	wxLogMessage(_T("File: %s\nFileCount: %i\nData Offset: %i\nFile Length: %i\n"), 
		filename, header.indexCount, header.dataOffset, header.fileSize);

	// Clear and Create the index
	wxDELETEA(index);
	index = new CacheIndex[header.indexCount];
	cacheFile.read((char*)index, (sizeof(CacheIndex) * header.indexCount));
     
	ofstream FileA(fn.AfterLast('\\')+".xml");
	FileA << "<?xml version=\"1.0\"?>" << endl;
	FileA << "\t<Indexes>" << endl;
	for(unsigned int i =0; i < header.indexCount; i++)
	{
		FileA << "\t\t<Index Id=\"" << index[i].id << "\">" << endl;
		FileA << "\t\t\t<Offset>"<< index[i].offset << "</Offset>" << endl;
		FileA << "\t\t\t<Rawsize>" << index[i].rawSize << "</Rawsize>" << endl;
		FileA << "\t\t\t<Size>" << index[i].size << "</Size>" << endl;
		FileA << "\t\t</Index>" << endl;
	}
	FileA << "\t</Indexes>" << endl;
	FileA.close();
}

unsigned int CacheArchive::getFileCount()
{
	// if file isn't open return 0
	if (!cacheFile.is_open())
	{
		return 0;
	}	
	return header.indexCount;
}

unsigned char* CacheArchive::getFile(unsigned int fileIndex)
{
	// make sure file is open 
	if (!cacheFile.is_open())
	{
		return 0;
	}

	// Error check
	if (fileIndex >= header.indexCount) 
	{
		wxLogMessage(_T("Error: Cannot load file index #%i, this cache archive doesn't have that many files!"), fileIndex);
		return 0;
	}

	// Error check
	if ((index[fileIndex].id>0) && (index[fileIndex].offset>0) && (index[fileIndex].rawSize>0) && (index[fileIndex].size>0)) {
		//wxLogMessage(_T("\n----\nFile Index: %i\nID: %i\nIndex Data Offset: %i\nUncompressed Size: %i\nCompressed Size: %i"), fileIndex, index[fileIndex].id, index[fileIndex].offset, index[fileIndex].rawSize, index[fileIndex].size);
	} else {
		wxLogMessage(_T("Error loading fileindex: %i"), fileIndex);
		return 0;
	}

	// clear buffer from previous file
	wxDELETEA(buffer);

	// Allocate room in the buffer
	buffer = new unsigned char[index[fileIndex].size];

	// Position the file pointer to the data we want.
	cacheFile.seekg(index[fileIndex].offset, std::ios::beg);

	// Read the data into our buffer
	cacheFile.read((char*)buffer, index[fileIndex].size);
	
	if (index[fileIndex].size == index[fileIndex].rawSize)
	{
		// If the file isn't compressed, return the raw data
		return buffer;
	} 
	else 
	{
		// else decompress it
		// Create memory streams store the data.
		wxMemoryInputStream inputStream(buffer, index[fileIndex].size);
		wxMemoryOutputStream memStream;
		wxZlibInputStream *zOutput = new wxZlibInputStream(inputStream, wxZLIB_ZLIB); //wxZLIB_AUTO); 

		// read the string into the zlib stream 
		zOutput->Read(memStream);

		// IMPORTANT: Force flush and zip finalization to our memory buffer stream 
		wxDELETE(zOutput);

		// get data size
		size_t streamSize = memStream.GetSize(); 

		// Clear previous buffer
		wxDELETEA(outBuffer);
		outBuffer = new char[streamSize]; 

		// copy to out buffer
		std::memset(outBuffer, 60, streamSize); 
		size_t numCopied = memStream.CopyTo(outBuffer, streamSize);

		// Error check
		if (streamSize != (size_t)index[fileIndex].rawSize)
		{
			wxLogMessage(_T("Error: Uncompressed stream data is not the same size that was expected!\nThe data may have been corrupted."));
		}

		if (numCopied != streamSize) 
		{
			wxLogMessage(_T("Error: Uncompressed stream data was not completely copied to the buffer!"));
			return NULL; 
		}
		
		// Output file
		//FILE *fileOutput;

		// output uncompressed data
		/*fileOutput = fopen("c:\\uncompressed.dat", "wb+");
		if (fileOutput) {
			fwrite(outBuffer, 1, numCopied, fileOutput);
			fclose(fileOutput);
		}
		*/

		return (unsigned char*)outBuffer;
	}

	// If we get here, return null - something bad must of happened.
	return 0;
}

unsigned int CacheArchive::getFileSize(unsigned int fileIndex)
{
	return index[fileIndex].rawSize;
}

unsigned int CacheArchive::getFileID(unsigned int fileIndex)
{
	if (!index)
		return 0;

	return index[fileIndex].id;
}

unsigned int CacheArchive::getFileSizeByID(unsigned int id)
{
	int fileIndex = -1;

	// Loop through the index until we find the file id we want
	for (unsigned int i=0; i<header.indexCount; i++) {
		if (index[i].id == id) {
			fileIndex = i;
			break;
		}
	}

	if (fileIndex > -1)
		return index[fileIndex].rawSize;
	
	return 0;
}

unsigned char* CacheArchive::getFileByID(unsigned int id)
{
	// make sure file is open 
	if (!cacheFile.is_open()) {
		return 0;
	}

	// clear buffer from previous file
	wxDELETEA(buffer);

	int fileIndex = -1;

	// Loop through the index until we find the file id we want
	for (unsigned int i=0; i<header.indexCount; i++) {
		if (index[i].id == id) {
			fileIndex = i;
			break;
		}
	}

	// Error check
	if (fileIndex == -1) {
		// We didn't find the file ?
		wxLogMessage(_T("Error: Couldn't find the file associated with ID #%i."), id);
		return 0;
	}

	// Allocate room in the buffer
	buffer = new unsigned char[index[fileIndex].size];

	// Position the file pointer to the data we want.
	cacheFile.seekg(index[fileIndex].offset, std::ios::beg);

	// Read the data into our buffer
	cacheFile.read((char*)buffer, index[fileIndex].size);
	
	if (index[fileIndex].size == index[fileIndex].rawSize) 
	{
		// If the file isn't compressed, return the raw data
		return buffer;
	} 
	else 
	{
		// else decompress it
		// Create memory streams to store the data.
		wxMemoryInputStream *inputStream = new wxMemoryInputStream(buffer, index[fileIndex].size);

		// Create the ZLib stream from our memory stream that has the data, this zlib stream automatically decompresses the data on creation
		wxZlibInputStream *zOutput = new wxZlibInputStream(*inputStream, wxZLIB_ZLIB); //wxZLIB_AUTO); 

		// Our Output memory stream
		wxMemoryOutputStream memStream;
	
		// Writes that data into our "output" memory stream.
		zOutput->Read(memStream);

		// Destroy our input and zlib memory streams,  our output stream has the data we're interested in.
		wxDELETE(inputStream);
		wxDELETE(zOutput);

		// get data size
		size_t streamSize = memStream.GetSize(); 

		// Clear previous buffer
		wxDELETEA(outBuffer);
		outBuffer = new char[streamSize]; 

		// copy to out buffer
		size_t numCopied = memStream.CopyTo(outBuffer, streamSize);

		// Error check
		if (streamSize != (size_t)index[fileIndex].rawSize)
			wxLogMessage(_T("Error: Uncompressed stream data is not the same size that was expected!\nThe data may have been corrupted."));

		// Error check
		if (numCopied != streamSize) { 
			wxLogMessage(_T("Error: Uncompressed stream data was not fully copied to buffer!"));
			return NULL; 
		}
		
		// return our decompressed data.
		return (unsigned char*)outBuffer;
	}


	// If we get here then something screwed up, return null
	return 0;
}

void CacheArchive::close()
{
	if (cacheFile.is_open()) {
		cacheFile.close();
	}

	wxDELETEA(buffer);
	wxDELETEA(outBuffer);
	wxDELETEA(index);
}

CacheArchive::~CacheArchive()
{
	if (cacheFile.is_open()) {
		cacheFile.close();
	}

	wxDELETEA(buffer);
	wxDELETEA(outBuffer);
	wxDELETEA(index);
}
