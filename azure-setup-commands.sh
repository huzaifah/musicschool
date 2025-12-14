#!/bin/bash
# Azure Service Principal Setup Commands
# Run these commands to set up Azure authentication for GitHub Actions

# Step 1: Login to Azure (this will open a browser)
az login

# Step 2: Get your subscription ID (copy the "id" value from the output)
az account show --query id -o tsv

# Step 3: Create service principal for GitHub Actions
# Replace <SUBSCRIPTION_ID> with the value from step 2
az ad sp create-for-rbac \
  --name "github-musicschool-deploy" \
  --role contributor \
  --scopes /subscriptions/<SUBSCRIPTION_ID>/resourceGroups/rg-musicschool-prod/providers/Microsoft.Web/sites/nadiritma \
  --sdk-auth

# The output will be a JSON object - copy the ENTIRE output
# You'll add this as a GitHub secret named AZURE_CREDENTIALS
