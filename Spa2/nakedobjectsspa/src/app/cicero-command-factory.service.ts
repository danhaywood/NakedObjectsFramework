﻿import { Injectable } from '@angular/core';
import { Command } from './cicero-commands';
import * as Cmd from './cicero-commands';
import * as Msg from './user-messages';
import { ContextService } from './context.service';
import { UrlManagerService } from './url-manager.service';
import { MaskService } from './mask.service';
import { ErrorService } from './error.service';
import { ConfigService } from './config.service';
import { Location } from '@angular/common';
import { Dictionary } from 'lodash';
import filter from 'lodash/filter';
import reduce from 'lodash/reduce';
import last from 'lodash/last';
import map from 'lodash/map';
import * as Cicerocontextservice from './cicero-context.service';

export class ParseResult {
    command?: Command[];
    error?: string;
}


@Injectable()
export class CiceroCommandFactoryService {

    constructor(protected urlManager: UrlManagerService,
        protected location: Location,
        protected context: ContextService,
        protected mask: MaskService,
        protected error: ErrorService,
        protected configService: ConfigService,
        protected ciceroContext : Cicerocontextservice.CiceroContextService) { }

    private commandsInitialised = false;

    private commands: Dictionary<Command> = {
        "ac": new Cmd.Action(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ba": new Cmd.Back(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ca": new Cmd.Cancel(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "cl": new Cmd.Clipboard(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ed": new Cmd.Edit(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "en": new Cmd.Enter(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "fo": new Cmd.Forward(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ge": new Cmd.Gemini(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "go": new Cmd.Goto(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "he": new Cmd.Help(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "me": new Cmd.Menu(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ok": new Cmd.OK(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "pa": new Cmd.Page(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "re": new Cmd.Reload(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "ro": new Cmd.Root(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "sa": new Cmd.Save(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "se": new Cmd.Selection(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "sh": new Cmd.Show(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext),
        "wh": new Cmd.Where(this.urlManager, this.location, this, this.context, this.mask, this.error, this.configService, this.ciceroContext)
    };


  

    getCommandNew(input: string): ParseResult {
        //TODO: sort out whether to process CVMs here, or in the execute method  -  is this ALWAYS called first?
        //Also, must not modify the *input* one here
        //cvm.chainedCommands = null; //TODO: Maybe not needed if unexecuted commands are cleared down upon error?
        if (!input) { //Special case for hitting Enter with no input
            return { command: [this.getCommand("wh")] };
        }

        try {
            const commands = input.split(";");
            const cmds = map(commands, (c) => this.getSingleCommand(c, commands.length > 1));
            return { command: cmds };

        } catch (e) {
            return { error: e.message }
        }

      
    };

    getSingleCommand = (input: string, chained: boolean) => {

        input = input.trim();
        const firstWord = input.split(" ")[0].toLowerCase();
        const command = this.getCommand(firstWord);
        let argString: string | null = null;
        const index = input.indexOf(" ");
        if (index >= 0) {
            argString = input.substr(index + 1);
        }
        command.argString = argString;
        command.chained = chained;

        return command;
    };


   

    //TODO: change the name & functionality to pre-parse or somesuch as could do more than auto
    //complete e.g. reject unrecognised action or one not available in context.
    autoComplete = (input: string) : {in : string, out : string} => {
        if (!input) {
            return {in : input, out : null};
        }
        let lastInChain = last(input.split(";")).toLowerCase();
        const charsTyped = lastInChain.length;
        lastInChain = lastInChain.trim();
        if (lastInChain.length === 0 || lastInChain.indexOf(" ") >= 0) { //i.e. not the first word
            //cvm.input += " ";
            return { in: input + " ", out: null };;
        }
        try {
            const command = this.getCommand(lastInChain);
            const earlierChain = input.substr(0, input.length - charsTyped);
           // return earlierChain + command.fullCommand + " ";
            return { in: earlierChain + command.fullCommand + " ", out: null };;
        } catch (e) {
            return {in : "", out: e.message}
        }
    };

    getCommand = (commandWord: string) => {
        if (commandWord.length < 2) {
            throw new Error(Msg.commandTooShort);
        }
        const abbr = commandWord.substr(0, 2);
        const command = this.commands[abbr];
        if (command == null) {
            throw new Error(Msg.noCommandMatch(abbr));
        }
        command.checkMatch(commandWord);
        return command;
    };

    allCommandsForCurrentContext = () => {
        const commandsInContext = filter(this.commands, c => c.isAvailableInCurrentContext());
        return reduce(commandsInContext, (r, c) => r + c.fullCommand + "\n", Msg.commandsAvailable);
    };

}
