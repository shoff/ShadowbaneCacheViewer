
#include "zones.h"


CZone::CZone(unsigned int fileID)
{

	unsigned char *buf = zoneArchive.getFileByID(fileID);

	if (buf) {
		wxLogMessage(_T("Loading Zone ID: %i"), fileID);
		Load(buf);
	}
}


void CZone::Load(const unsigned char *buffer)
{
	unsigned int ptr = 0;
	wxChar letter = 0;
	unsigned int counter = 0;
	unsigned int value = 0;
	float fVal = 0.0f;

	// ------------------------------
	memcpy(&value, buffer+ptr, 4);
	ptr += 4;

	memcpy(&counter, buffer+ptr, 4);
	ptr += 4;

	for (unsigned int i=0; i<counter; i++) {
		memcpy(&letter, buffer+ptr+(i*2), sizeof(wxChar));
		name.Append(letter);
	}
	ptr += counter*2;

	// Debug info
	wxLogMessage(_T("Loading Zone: %s,  value: %i"), name.c_str(), value);
	// ------

	
	// Skip over unknown data
	ptr += 254;


	memcpy(&counter, buffer+ptr, 4);
	ptr += 4;

	for (unsigned int i=0; i<counter; i++) {
		ptr += 4;	// skip over null

		memcpy(&value, buffer+ptr, 4);
		ptr += 4;

		props.push_back(new CObject(value));

		// -
		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		ptr += 4;

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		// null byte here? or at the end?
		// ptr++;

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		memcpy(&fVal, buffer+ptr, 4);
		ptr += 4;
		wxLogMessage(_T("%f"), fVal);

		// null byte
		ptr++;
	}
}



CZone::~CZone()
{
	
}

// ---------