using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using log4net;
using WinSECS.structure;
using WinSECS.global;
using WinSECS.Utility;


namespace WinSECS.MessageHandler
{
    [ComVisible(false)]
    public class Dispatcher : DispatcherModelingFactory
    {
        private ILog logger;

        public Dispatcher(ILog logger)
        {
            this.logger = logger;
        }

        private void FillItemNameofListFormatWithItemKey(IFormatCollection receivedList, IFormatCollection modelingList, ReturnObject returnObject)
        {
            IEnumerator<IFormat> enumerator = receivedList.GetEnumerator();
            IEnumerator<IFormat> enumerator2 = modelingList.GetEnumerator();
            while (enumerator.MoveNext() && enumerator2.MoveNext())
            {
                IFormat current = enumerator.Current;
                IFormat modeledItem = enumerator2.Current;
                byte type = current.Type;
                byte num2 = modeledItem.Type;
                if (type == num2)
                {
                    string str;
                    if (type == 0)
                    {
                        if (!(!modeledItem.ItemKey || this.IsValidItemKey(modeledItem, current)))
                        {
                            str = string.Format("{0} Itemkey={1} Keyvalue={2}", SEComError.SEComMessageHanlder.getErrDescription(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY), modeledItem.Name, modeledItem.Value);
                            returnObject.setError(str);
                            break;
                        }
                        current.Name = modeledItem.Name;
                        if (modeledItem.Variable)
                        {
                            this.FillItemNameofVariableListFormatWithItemKey(((ListFormat)current).Children, ((ListFormat)modeledItem).Children, current.Length, returnObject);
                            if (!returnObject.isSuccess())
                            {
                                break;
                            }
                        }
                        else
                        {
                            this.FillItemNameofListFormatWithItemKey(((ListFormat)current).Children, ((ListFormat)modeledItem).Children, returnObject);
                            if (!returnObject.isSuccess())
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!(!modeledItem.ItemKey || this.IsValidItemKey(modeledItem, current)))
                        {
                            str = string.Format("{0} Itemkey={1} Keyvalue={2}", SEComError.SEComMessageHanlder.getErrDescription(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY), modeledItem.Name, modeledItem.Value);
                            returnObject.setError(str);
                            break;
                        }
                        current.Name = modeledItem.Name;
                    }
                }
                else if (modeledItem.Type != 0x7f)
                {
                    returnObject.setError(string.Concat(new object[] { "Modeled Format=", modeledItem.Type, " Received Format=", current.Type }));
                    break;
                }
            }
        }

        private void FillItemNameofVariableListFormatWithItemKey(IFormatCollection receivedList, IFormatCollection modelingList, int nReceivedLength, ReturnObject returnObject)
        {
            string str;
            IEnumerator<IFormat> enumerator = receivedList.GetEnumerator();
            IEnumerator<IFormat> enumerator2 = modelingList.GetEnumerator();
            IFormat modeledItem = enumerator2.MoveNext() ? enumerator2.Current : null;
            if (modeledItem == null)
            {
                if (nReceivedLength > 0)
                {
                    str = "During Dispatching, Not yet defined Error Occures, Please Send MSG Format(SMD/LOG) to AIM's Developer";
                    returnObject.setError(str);
                }
            }
            else
            {
                for (int i = 0; i < nReceivedLength; i++)
                {
                    if (enumerator.MoveNext())
                    {
                        IFormat current = enumerator.Current;
                        byte type = current.Type;
                        byte num3 = modeledItem.Type;
                        if (type == num3)
                        {
                            if (type == 0)
                            {
                                if (!(!modeledItem.ItemKey || this.IsValidItemKey(modeledItem, current)))
                                {
                                    str = string.Format("{0} Itemkey={1} Keyvalue={2}", SEComError.SEComMessageHanlder.getErrDescription(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY), modeledItem.Name, modeledItem.Value);
                                    returnObject.setError(str);
                                    break;
                                }
                                current.Name = modeledItem.Name;
                                if (modeledItem.Variable)
                                {
                                    this.FillItemNameofVariableListFormatWithItemKey(((ListFormat)current).Children, ((ListFormat)modeledItem).Children, current.Length, returnObject);
                                    if (!returnObject.isSuccess())
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    this.FillItemNameofListFormatWithItemKey(((ListFormat)current).Children, ((ListFormat)modeledItem).Children, returnObject);
                                    if (!returnObject.isSuccess())
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (!(!modeledItem.ItemKey || this.IsValidItemKey(modeledItem, current)))
                                {
                                    str = string.Format("{0} Itemkey={1} Keyvalue={2}", SEComError.SEComMessageHanlder.getErrDescription(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_WRONG_ITEMKEY), modeledItem.Name, modeledItem.Value);
                                    returnObject.setError(str);
                                    break;
                                }
                                current.Name = modeledItem.Name;
                            }
                        }
                        else if (modeledItem.Type != 0x7f)
                        {
                            returnObject.setError(string.Concat(new object[] { "Modeled Format=", modeledItem.Type, " Received Format=", current.Type }));
                            break;
                        }
                    }
                }
            }
        }

        public void FillWithModelingMessage(SECSTransaction receivedTrx, SECSTransaction modelingTrx, ReturnObject returnObject)
        {
            receivedTrx.MessageName = modelingTrx.MessageName;
            receivedTrx.Autoreply = modelingTrx.Autoreply;
            receivedTrx.IsLogging = modelingTrx.IsLogging;
            this.FillItemNameofListFormatWithItemKey(receivedTrx.Children, modelingTrx.Children, returnObject);
        }

        public virtual void GetAdaptableMessage(SECSTransaction receivedTrx, ReturnObject returnObject)
        {
            Exception exception;
            IDictionary<string, object> dictionary = base.modelingFactoryWithItemKey.getBigCategory(string.Format("S{0}F{1}", receivedTrx.Stream, receivedTrx.Function));
            if (dictionary != null)
            {
                foreach (SECSTransaction transaction in dictionary.Values)
                {
                    try
                    {
                        if (this.IsAdaptable(receivedTrx, transaction))
                        {
                            this.FillWithModelingMessage(receivedTrx, transaction, returnObject);
                            if (returnObject.isSuccess())
                            {
                                returnObject.setReturnData(receivedTrx);
                                return;
                            }
                            this.logger.Debug(returnObject.getErrorObject().getErrorDiscription());
                            returnObject.setError(0);
                        }
                    }
                    catch (Exception exception1)
                    {
                        exception = exception1;
                        this.logger.Warn("Exception Occured in 1st Comparing Item with Key DefinedMessage is " + transaction.Name + " ReceivedMessage is " + receivedTrx.HeaderString + ConstUtils.NEWLINE + receivedTrx.SECS1BodyString);
                    }
                }
            }
            dictionary = base.modelingFacotry.getBigCategory(string.Format("S{0}F{1}", receivedTrx.Stream, receivedTrx.Function));
            if (dictionary == null)
            {
                returnObject.setError(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_BIG_CATEGORY);
            }
            else
            {
                foreach (SECSTransaction transaction in dictionary.Values)
                {
                    try
                    {
                        if (this.IsAdaptable(receivedTrx, transaction))
                        {
                            this.FillWithModelingMessage(receivedTrx, transaction, returnObject);
                            if (returnObject.isSuccess())
                            {
                                returnObject.setReturnData(receivedTrx);
                                return;
                            }
                            this.logger.Debug(returnObject.getErrorObject().getErrorDiscription());
                            returnObject.setError(0);
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        this.logger.Warn("Exception Occured in 2nd Comparing Item with Key DefinedMessage is " + transaction.Name + " ReceivedMessage is " + receivedTrx.HeaderString + ConstUtils.NEWLINE + receivedTrx.SECS1BodyString);
                    }
                }
                returnObject.setError(SEComError.SEComMessageHanlder.NO_MATCH_MODELING_MESSAGE_SMALL_CATEGORY);
            }
        }

        private bool IsAdaptable(SECSTransaction receivedTrx, SECSTransaction modelingTrx)
        {
            string regexPattern = modelingTrx.GetRegexPattern();
            string regexInput = receivedTrx.GetRegexInput();
            if (regexPattern.Equals(""))
            {
                return regexInput.Equals("");
            }
            if (regexInput.Length > 50)
            {
                if (receivedTrx.Children.Count > modelingTrx.Children.Count)
                {
                    this.logger.Debug(string.Concat(new object[] { "List Data Too long! Modeling Trx Name=", modelingTrx.Name, " List=", modelingTrx.Children.Count, " ReceivedList=", receivedTrx.Children.Count }));
                    return false;
                }
                if (receivedTrx.Children.Count < modelingTrx.Children.Count)
                {
                    this.logger.Debug(string.Concat(new object[] { "List Data Too Short! Modeling Trx Name=", modelingTrx.Name, " List=", modelingTrx.Children.Count, " ReceivedList=", receivedTrx.Children.Count }));
                    return false;
                }
                ReturnObject returnObject = new ReturnObject();
                int num = 0;
                foreach (IFormat format in modelingTrx.Children)
                {
                    if ((format.Type == ListFormat.TYPE) && (receivedTrx.Children[num].Type == ListFormat.TYPE))
                    {
                        returnObject = ((ListFormat)receivedTrx.Children[num]).compareRegularPattern(format as ListFormat, returnObject);
                        if (!returnObject.isSuccess())
                        {
                            this.logger.Debug(string.Concat(new object[] { "Received S", receivedTrx.Stream, "F", receivedTrx.Function, " Dispatching Result: ", returnObject.getErrorDescription() }));
                            return false;
                        }
                    }
                    else if (!Regex.Match(receivedTrx[num].GetRegexInput(), format.GetRegexPattern()).Success)
                    {
                        this.logger.Debug("Modeled Transaction GetRegxPattern()=" + format.GetRegexPattern() + " Received Transaction GetRegexInput()=" + receivedTrx[num].GetRegexInput());
                        return false;
                    }
                    num++;
                }
            }
            else if (!Regex.Match(regexInput, regexPattern).Success)
            {
                this.logger.Debug("Modeled Transaction GetRegxPattern()=" + regexPattern + "Received Transaction GetRegexInput()=" + regexInput);
                return false;
            }
            return true;
        }

        private bool IsValidItemKey(IFormat modeledItem, IFormat receivedItem)
        {
            bool flag = false;
            if (modeledItem.Value.Equals(receivedItem.Value))
            {
                return true;
            }
            if (modeledItem.Value.Equals("") || receivedItem.Value.Equals(""))
            {
                return false;
            }
            string[] strArray = modeledItem.Value.Split(new char[] { ',' });
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2;
                int num2;
                int num3;
                int num4;
                Exception exception;
                string key = strArray[i];
                if (key.StartsWith("~"))
                {
                    key = key.Replace("~", "");
                    if (key.IndexOf("-") > 0)
                    {
                        try
                        {
                            strArray2 = key.Split(new char[] { '-' });
                            if (strArray2.Length == 2)
                            {
                                num2 = int.Parse(strArray2[0]);
                                num3 = int.Parse(strArray2[1]);
                                if (num3 >= num2)
                                {
                                    num4 = num2;
                                    while (num4 <= num3)
                                    {
                                        dictionary.Remove(num4.ToString());
                                        num4++;
                                    }
                                }
                            }
                        }
                        catch (Exception exception1)
                        {
                            exception = exception1;
                            dictionary.Remove(key);
                        }
                    }
                    else
                    {
                        dictionary.Remove(key);
                    }
                }
                else if (key.IndexOf("-") > 0)
                {
                    try
                    {
                        strArray2 = key.Split(new char[] { '-' });
                        if (strArray2.Length == 2)
                        {
                            num2 = int.Parse(strArray2[0]);
                            num3 = int.Parse(strArray2[1]);
                            if (num3 >= num2)
                            {
                                for (num4 = num2; num4 <= num3; num4++)
                                {
                                    string introduced15 = num4.ToString();
                                    dictionary.Add(introduced15, num4.ToString());
                                }
                            }
                        }
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        dictionary.Add(key, key);
                    }
                }
                else
                {
                    dictionary.Add(key, key);
                }
            }
            if (dictionary.ContainsKey(receivedItem.Value))
            {
                flag = true;
            }
            dictionary.Clear();
            return flag;
        }

        public ILog Logger
        {
            set
            {
                this.logger = value;
            }
        }
    }
}
