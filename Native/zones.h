
#pragma once

#include "stdafx.h"
#include "objects.h"

class CZone {
private:
	wxString name;
	
	std::vector<CObject*> props;

public:
	CZone();
	CZone(unsigned int fileID);
	~CZone();

	void Load(const unsigned char *buffer);


};




// -----