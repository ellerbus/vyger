# Code Generator for Vyger

Uses in-memory sqlite database to gen files from

## Setup Notes for Development

```powershell
# cd to Vyger-console
py -m venv ./env

# Start/Active virtual environment
./env/scripts/activate.ps1

# upgrade pip
py -m pip install --upgrade pip

# install dependencies
pip install -r requirements.txt

# quick test to see if everything works
py main.py -t table_name -x template_name (or wildcard*template_name)
```
