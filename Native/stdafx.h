// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

// wxWidgets precompiled
// #include "wx/wxprec.h"
//#ifndef WX_PRECOMP
    #include "wx/wx.h"
//#endif

// wxWidgets
#include <wx/treectrl.h>
#include <wx/glcanvas.h>
#include <wx/zstream.h>
#include <wx/zipstrm.h>
#include <wx/stream.h>
#include <wx/mstream.h>
#include <wx/sstream.h>
#include <wx/wfstream.h>
#include <wx/debugrpt.h>
#include <wx/log.h>
#include <wx/arrstr.h>
#include <wx/statbmp.h>
#include <wx/image.h>

// wxAUI
#include "manager.h"

// C/C++ headers
#include <tchar.h>

// STL
#include <set>
#include <vector>
#include <iostream>
#include <fstream>

// OpenGL
#include <GL/gl.h>
#include <GL/glu.h>

// FMod
//#include <fmod.h>

// our custom stuff
#include "enums.h"
#include "ximage.h"
#include "vec3d.h"
#include "vec4d.h"

typedef unsigned __int32 uint32;
typedef __int32 int32;
typedef unsigned __int16 uint16;
typedef __int16 int16;